using ApplicationSettings;
using Azure.Messaging.ServiceBus;
using Companion.Modules.Extensions.InstantMessaging.DTOs;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Companion.Modules.Extensions.InstantMessaging.Providers
{
	public class ServiceBusInstantMessagingProvider
	{
		private AzureServiceBusProvider _azureServiceBusProvider;

		private AppSettings _appSettings;

		private ConcurrentDictionary<Guid, IInstantMessage> messageInbox;

		public ServiceBusInstantMessagingProvider(AppSettings appSettings)
		{
			_appSettings = appSettings;
			messageInbox = new ConcurrentDictionary<Guid, IInstantMessage>();
			var outboundTopicName = $"to_{_appSettings!.AppData!.UserName!.ToLower()}";
			var inboundTopicName = $"{_appSettings!.AppData!.UserName!.ToLower()}-subscriber";
			_azureServiceBusProvider = new AzureServiceBusProvider(appSettings);
			_azureServiceBusProvider.AddMessageListener(outboundTopicName, inboundTopicName, (messageHandler) => InboxInstantMessage(messageHandler), (x) => errorhand(x));
			_azureServiceBusProvider.CreateTopicIfNotExists(outboundTopicName);
		}

		public void SendInstantMessage(IInstantMessage instantMessage)
		{
			var receiverTopicName = $"to_{instantMessage?.ToUser?.ToLower()}";
			var receiverSubscriptionName = $"{instantMessage?.ToUser?.ToLower()}-subscriber";
			_azureServiceBusProvider.CreateTopicIfNotExists(receiverTopicName);
			_azureServiceBusProvider.
			CreateSubscriptionIfNotExists(receiverTopicName, receiverSubscriptionName);
			_azureServiceBusProvider.SendMessage(receiverTopicName, JsonSerializer.Serialize(instantMessage));
		}

		/// <summary>
		/// Prints messages from a specified user
		/// </summary>
		/// <param name="commandParameters">should contain only a from user search param</param>
		public void PrintInstantMessages(string[] commandParameters)
		{
			if (commandParameters.Any())
			{
				var fromUser = commandParameters[0];
				var matchedMessages = messageInbox.Where(message => string.Compare(message.Value.FromUser, fromUser, true) == 0).Select(message => message.Value).ToList();
				if (matchedMessages.Any())
				{
					PrintMessages(matchedMessages);
				}
				else
				{
					Console.WriteLine($"No messages from user {fromUser} were found.");
				}
			}
			else
			{
				Console.WriteLine($"No parameters were defined.");
			}
		}

		/// <summary>
		/// Prints all messages in the inbox
		/// </summary>
		public void PrintInstantMessages()
		{
			PrintMessages(messageInbox.Select(message => message.Value).ToList());
		}

		private void PrintMessages(List<IInstantMessage> instantMessages)
		{
			foreach (var instantMessage in instantMessages)
			{
				Console.WriteLine($"{instantMessage.FromUser} says: {instantMessage.Message}");
			}

		}

		private async Task InboxInstantMessage(ProcessMessageEventArgs messageHandler)
		{
			var instantMessage = JsonSerializer.Deserialize<InstantMessage>(messageHandler.Message.Body.ToString());
			messageInbox.TryAdd(Guid.NewGuid(), instantMessage);
			await messageHandler.CompleteMessageAsync(messageHandler.Message);
		}

		private async Task errorhand(ProcessErrorEventArgs messageHandler)
		{
			var x = messageHandler.ErrorSource;
		}
	}
}
