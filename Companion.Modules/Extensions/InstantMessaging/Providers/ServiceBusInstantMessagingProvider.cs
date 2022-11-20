using ApplicationSettings;
using Azure.Messaging.ServiceBus;
using Companion.Modules.Extensions.InstantMessaging.DTOs;
using Companion.Modules.Extensions.InstantMessaging.POCOs;
using System.Text.Json;

namespace Companion.Modules.Extensions.InstantMessaging.Providers
{
	/// <summary>
	/// This class will create the needed topics and subscriptions on the service bus for messaging. 
	/// It will also update the users inbox as messages come in. 
	/// </summary>
	public class ServiceBusInstantMessagingProvider
	{
		private string outboundTopicName;

		private string inboundPersonalTopicName;

		private string inboundAllTopicName;

		private string outboundAllTopicName;

		private AzureServiceBusProvider _azureServiceBusProvider;

		private AppSettings _appSettings;

		private MessageInbox _messageInbox;


		/// <summary>
		/// In order to await topic initializiation make the constructor private and provide an async dreate method.
		/// </summary>
		public static async Task<ServiceBusInstantMessagingProvider> CreateAsync(AppSettings appSettings, MessageInbox messageInbox)
		{
			var serviceBusInstantMessagingProvider = new ServiceBusInstantMessagingProvider(appSettings, messageInbox);
			await serviceBusInstantMessagingProvider.InitializeTopicsAndSubscriptions();
			return serviceBusInstantMessagingProvider;
		}

		private ServiceBusInstantMessagingProvider(AppSettings appSettings, MessageInbox messageInbox)
		{
			_appSettings = appSettings;
			_messageInbox = messageInbox;
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

		private async Task InboxInstantMessage(ProcessMessageEventArgs messageHandler)
		{
			var instantMessage = JsonSerializer.Deserialize<InstantMessage>(messageHandler.Message.Body.ToString()) ?? new InstantMessage();
			// Don't add messages sent to the all topic from this user
			if (string.Compare(instantMessage.FromUser, _appSettings.AppData.UserName, true) != 0)
			{
				_messageInbox.TryAddMessage(instantMessage);
			}
			await messageHandler.CompleteMessageAsync(messageHandler.Message);
		}

		private async Task InboxErrorHandler(ProcessErrorEventArgs messageHandler)
		{
			EventLogger.WriteErrorToEventLog($"Exception processing service bus message {messageHandler.Exception} ");
		}
	}
}
