using ApplicationSettings;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace Companion.Modules.Extensions.InstantMessaging.Providers
{
	/// <summary>
	/// This class contains all the commands needed for interacting directly with the service bus.
	/// </summary>
	public class AzureServiceBusProvider
	{
		private ServiceBusClient _client;

		private ServiceBusAdministrationClient _adminClient;

		private AppSettings _appSettings;

		public AzureServiceBusProvider(AppSettings appSettings)
		{
			_appSettings = appSettings;
			var clientOptions = new ServiceBusClientOptions()
			{
				TransportType = ServiceBusTransportType.AmqpWebSockets
			};

			_client = new ServiceBusClient(_appSettings.AppData.AzureServiceBusConnectionString, clientOptions);
			_adminClient = new ServiceBusAdministrationClient(_appSettings.AppData.AzureServiceBusConnectionString);

		}

		public async Task CreateTopicIfNotExists(string topicName)
		{
			if (!await _adminClient.TopicExistsAsync(topicName))
			{
				await _adminClient.CreateTopicAsync(topicName);
			}
		}

		public async Task CreateSubscriptionIfNotExists(string topicName, string subscriptionName)
		{
			if (!await _adminClient.SubscriptionExistsAsync(topicName, subscriptionName))
			{
				await _adminClient.CreateSubscriptionAsync(topicName, subscriptionName);
			}
		}

		public async Task SendMessage(string topicName, string message)
		{
			var _sender = _client.CreateSender(topicName);
			await _sender.SendMessageAsync(new ServiceBusMessage(message));
			await _sender.CloseAsync();
		}

		public async Task AddMessageListener(string topicName, string subscriptionName, Func<ProcessMessageEventArgs, Task> messageHandler, Func<ProcessErrorEventArgs, Task> errhand)
		{
			var processor = _client.CreateProcessor(topicName, subscriptionName);
			processor.ProcessMessageAsync += messageHandler;
			processor.ProcessErrorAsync += errhand;
			await processor.StartProcessingAsync();
		}
	}
}
