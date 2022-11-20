using ApplicationSettings;
using Azure.Messaging.ServiceBus;
using Companion.Modules.Extensions.InstantMessaging.DTOs;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Companion.Modules.Extensions.InstantMessaging.Providers
{
	public class ServiceBusInstantMessagingProvider
	{
		private string outboundTopicName;

		private string inboundPersonalTopicName;

		private string inboundAllTopicName;

		private string outboundAllTopicName;

		private AzureServiceBusProvider _azureServiceBusProvider;

		private AppSettings _appSettings;

		private ConcurrentDictionary<Guid, IInstantMessage> messageInbox;

		/// <summary>
		/// In order to await topic initializiation make the constructor private and provide an async dreate method.
		/// </summary>
		public static async Task<ServiceBusInstantMessagingProvider> CreateAsync(AppSettings appSettings)
		{
			var serviceBusInstantMessagingProvider = new ServiceBusInstantMessagingProvider(appSettings);
			await serviceBusInstantMessagingProvider.InitializeTopicsAndSubscriptions();
			return serviceBusInstantMessagingProvider;
		}

		private ServiceBusInstantMessagingProvider(AppSettings appSettings)
		{
			_appSettings = appSettings;
			messageInbox = new ConcurrentDictionary<Guid, IInstantMessage>();
			outboundTopicName = $"to_{_appSettings!.AppData!.UserName!.ToLower()}";
			inboundPersonalTopicName = $"{_appSettings!.AppData!.UserName!.ToLower()}-subscriber";
			inboundAllTopicName = $"all-subscriber-{_appSettings!.AppData!.UserName!.ToLower()}";
			outboundAllTopicName = "to_all";
			_azureServiceBusProvider = new AzureServiceBusProvider(appSettings);
		}

		private async Task InitializeTopicsAndSubscriptions()
		{
			// Create topic for logged in user
			await _azureServiceBusProvider.CreateTopicIfNotExists(outboundTopicName);
			// Create topic for all users
			await _azureServiceBusProvider.CreateTopicIfNotExists(outboundAllTopicName);
			// Create subscription to the all topic
			await _azureServiceBusProvider.CreateSubscriptionIfNotExists(outboundAllTopicName, inboundAllTopicName);
			// Create subscription to the this user topic
			await _azureServiceBusProvider.CreateSubscriptionIfNotExists(outboundTopicName, inboundPersonalTopicName);
			// Subscribe to personal queue
			await _azureServiceBusProvider.AddMessageListener(outboundTopicName, inboundPersonalTopicName, (messageHandler) => InboxInstantMessage(messageHandler), (x) => InboxErrorHandler(x));
			// Subscribe to all queue
			await _azureServiceBusProvider.AddMessageListener(outboundAllTopicName, inboundAllTopicName, (messageHandler) => InboxInstantMessage(messageHandler), (x) => InboxErrorHandler(x));
		}

		public async Task SendInstantMessage(IInstantMessage instantMessage)
		{
			var receiverTopicName = $"to_{instantMessage?.ToUser?.ToLower()}";
			var receiverSubscriptionName = $"{instantMessage?.ToUser?.ToLower()}-subscriber";
			// Create topic for receiving user
			await _azureServiceBusProvider.CreateTopicIfNotExists(receiverTopicName);
			// Create subscription for receiving user so if they haven't logged in yet the messages don't vanish
			await _azureServiceBusProvider.CreateSubscriptionIfNotExists(receiverTopicName, receiverSubscriptionName);
			await _azureServiceBusProvider.SendMessage(receiverTopicName, JsonSerializer.Serialize(instantMessage));
		}

		/// <summary>
		/// Returns a list of 
		/// </summary>
		/// <param name="commandParameters">should contain only a from user search param, or no param</param>
		public IDictionary<Guid, IInstantMessage> GetInstantMessages(string? fromUser)
		{
			var matchedMessages = new Dictionary<Guid, IInstantMessage>();
			if (!string.IsNullOrWhiteSpace(fromUser))
			{
				matchedMessages = messageInbox.Where(message => string.Compare(message.Value.FromUser, fromUser, true) == 0).ToDictionary(key => key.Key, value => value.Value);
			}
			else
			{
				matchedMessages = messageInbox.ToDictionary(key => key.Key, value => value.Value);
			}

			RemoveMessagesFromInbox(matchedMessages);

			return matchedMessages;
		}

		private void RemoveMessagesFromInbox(IDictionary<Guid, IInstantMessage> instantMessages)
		{
			foreach (var instantMessage in instantMessages)
			{
				messageInbox.TryRemove(instantMessage.Key, out IInstantMessage removedMessage);
			}
		}

		private async Task InboxInstantMessage(ProcessMessageEventArgs messageHandler)
		{
			var instantMessage = JsonSerializer.Deserialize<InstantMessage>(messageHandler.Message.Body.ToString()) ?? new InstantMessage();
			// Don't add messages sent to the all topic from this user
			if (string.Compare(instantMessage.FromUser, _appSettings.AppData.UserName, true) != 0)
			{
				messageInbox.TryAdd(Guid.NewGuid(), instantMessage);
			}
			await messageHandler.CompleteMessageAsync(messageHandler.Message);
		}

		private async Task InboxErrorHandler(ProcessErrorEventArgs messageHandler)
		{
			var x = messageHandler.Exception;
		}
	}
}
