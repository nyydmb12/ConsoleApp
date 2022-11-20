using ApplicationSettings;
using Azure.Messaging.ServiceBus;
using Companion.Modules.Extensions.FinancialModule.Providers;
using Companion.Modules.Extensions.InstantMessaging.DTOs;
using Companion.Modules.Extensions.InstantMessaging.Providers;
using Microsoft.Azure.Amqp.Framing;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Companion.Modules.Extensions.InstantMessaging
{
	/// <summary>
	/// This class introduces the msg command for messaging.
	/// </summary>
	public class InstantMessagingModule : WelcomeModule
	{
		private AppSettings _appSettings;

		private const string ModuleKeyword = "msg";

		private ServiceBusInstantMessagingProvider _serviceBusInstantMessagingProvider;

		public InstantMessagingModule(ServiceBusInstantMessagingProvider serviceBusInstantMessagingProvider, AppSettings appSettings) : base(ModuleKeyword)
		{
			_appSettings = appSettings;
			_serviceBusInstantMessagingProvider = serviceBusInstantMessagingProvider;
			_availableCommands.AddCommand(new Command("send", "Send a message to another user, they must have initalized their message application first. msg send dan hello dan hows it going", (commandParameters) => SendInstantMessage(commandParameters)));
			_availableCommands.AddCommand(new Command("read", "Get messages from all other users. msg read", (commandParameters) => serviceBusInstantMessagingProvider.PrintInstantMessages()));
			_availableCommands.AddCommand(new Command("read name", "Get messages from specific other users. msg read dan", (commandParameters) => serviceBusInstantMessagingProvider.PrintInstantMessages(commandParameters)));
		}

		private void SendInstantMessage(string[] commandParameters)
		{
			var instantMessage = new InstantMessage()
			{
				FromUser = _appSettings!.AppData!.UserName,
				ToUser = commandParameters[0],
				Message = string.Join(' ', commandParameters.Skip(1))
			};
			_serviceBusInstantMessagingProvider.SendInstantMessage(instantMessage);
		}
	}
}