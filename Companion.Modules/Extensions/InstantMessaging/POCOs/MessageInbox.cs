using ApplicationSettings;
using Companion.Modules.Extensions.InstantMessaging.DTOs;
using System.Collections.Concurrent;

namespace Companion.Modules.Extensions.InstantMessaging.POCOs
{
	public class MessageInbox
	{
		private ConcurrentDictionary<Guid, IInstantMessage> _messageInbox;

		public Action _inboxObserver { private get; set; }

		public MessageInbox()
		{
			_inboxObserver = null;
			_messageInbox = new ConcurrentDictionary<Guid, IInstantMessage>();
		}

		public void TryAddMessage(IInstantMessage instantMessage)
		{
			_messageInbox.TryAdd(Guid.NewGuid(), instantMessage);

			if (_inboxObserver != null)
			{
				_inboxObserver();
			}

		}

		public void TryRemoveMessage(KeyValuePair<Guid, IInstantMessage> instantMessage)
		{
			_messageInbox.TryRemove(instantMessage);
		}

		/// <summary>
		/// Returns a list of 
		/// </summary>
		/// <param name="commandParameters">should contain only a from user search param, or no param</param>
		public IDictionary<Guid, IInstantMessage> GetInstantMessagesByUser(string? fromUser)
		{
			var matchedMessages = new Dictionary<Guid, IInstantMessage>();
			if (!string.IsNullOrWhiteSpace(fromUser))
			{
				matchedMessages = _messageInbox.Where(message => string.Compare(message.Value.FromUser, fromUser, true) == 0).ToDictionary(key => key.Key, value => value.Value);
			}
			else
			{
				matchedMessages = _messageInbox.ToDictionary(key => key.Key, value => value.Value);
			}

			return matchedMessages;
		}

	}
}
