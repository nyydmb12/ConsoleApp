using ApplicationSettings;
using Companion.Modules.Extensions.InstantMessaging.DTOs;
using Companion.Modules.Extensions.InstantMessaging.POCOs;
using Companion.Modules.Extensions.InstantMessaging.Providers;

namespace Companion.Modules.Extensions.InstantMessaging
{
	/// <summary>
	/// This class introduces the msg command for messaging.
	/// </summary>
	public class InstantMessagingModule : WelcomeModule
	{
		private AppSettings _appSettings;

		private const string _moduleKeyword = "msg";

		private ServiceBusInstantMessagingProvider _serviceBusInstantMessagingProvider;

		private MessageInbox _messageInbox;

		public InstantMessagingModule(ServiceBusInstantMessagingProvider serviceBusInstantMessagingProvider, MessageInbox messageInbox, AppSettings appSettings) : base(_moduleKeyword)
		{
			_messageInbox = messageInbox;
			_appSettings = appSettings;
			_serviceBusInstantMessagingProvider = serviceBusInstantMessagingProvider;
			_availableCommands.AddCommand(new Command("send", "Send a message to another user, they must have initalized their message application first. msg send dan hello dan hows it going", (commandParameters) => SendInstantMessage(commandParameters)));
			_availableCommands.AddCommand(new Command("read", "Get messages from all other users. msg read. You can all request messages from specific users msg read dan", (commandParameters) => PrintInboxMessages(commandParameters)));
			_messageInbox._inboxObserver = () => NotifyOnMessageReceived();
		}

		/// <summary>
		/// This method is an observer on the inbox
		/// </summary>
		private void NotifyOnMessageReceived()
		{
			Console.WriteLine("New Message received.");
		}

		private void SendInstantMessage(string[] commandParameters)
		{
			var instantMessage = new InstantMessage()
			{
				FromUser = _appSettings!.AppData!.UserName,
				ToUser = commandParameters[0],
				Message = string.Join(' ', commandParameters.Skip(1))
			};

			// No need to await message being sent
			_serviceBusInstantMessagingProvider.SendInstantMessage(instantMessage);
			Console.WriteLine($"Message sent to {instantMessage.ToUser}");
		}

		private void PrintInboxMessages(string[] commandParameters)
		{
			Dictionary<Guid, IInstantMessage> inboxMessages;
			var fromUser = commandParameters.Any() ? commandParameters[0] : null;
			inboxMessages = (Dictionary<Guid, IInstantMessage>)_messageInbox.GetInstantMessagesByUser(fromUser);
			if (inboxMessages.Any())
			{
				PrintMessages(inboxMessages);
			}
			else
			{
				if (string.IsNullOrWhiteSpace(fromUser))
				{
					Console.WriteLine($"No messages in your inbox.");
				}
				else
				{
					Console.WriteLine($"No messages from user {fromUser} were found.");
				}
			}
		}

		private void PrintMessages(IDictionary<Guid, IInstantMessage> instantMessages)
		{
			foreach (var instantMessage in instantMessages)
			{
				// If this wasn't sent to this user then it must be addressed to all users
				if (string.Compare(instantMessage.Value.ToUser, _appSettings.AppData.UserName, true) != 0)
				{
					Console.WriteLine($"{instantMessage.Value.FromUser} says to all users: {instantMessage.Value.Message}");
				}
				else
				{
					Console.WriteLine($"{instantMessage.Value.FromUser} says: {instantMessage.Value.Message}");
				}

				_messageInbox.TryRemoveMessage(instantMessage);
			}
		}
	}
}