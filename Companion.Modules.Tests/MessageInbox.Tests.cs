using Companion.Modules.Extensions.FinancialModule.Providers;
using Companion.Modules.Extensions.InstantMessaging.DTOs;
using Companion.Modules.Extensions.InstantMessaging.POCOs;
using Companion.Modules.Tests.ResponseMocks;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using Xunit;

namespace Companion.Modules.Tests
{
	public class MessageInbox_Tests
	{
		[Theory]
		[InlineData("president", 2)]
		[InlineData("vicepresident", 1)]
		[InlineData("", 3)]
		[InlineData(null, 3)]
		[InlineData("none", 0)]
		public async Task ShouldReturnExpectedNumberOfMessages(string inboxSearchParam, int expectedCount)
		{
			// Setup Inbox. Inbox should only have messages to dan and all
			var messageInbox = new MessageInbox();
			messageInbox.TryAddMessage(new InstantMessage() { FromUser = "president", Message = "test", ToUser = "all" });
			messageInbox.TryAddMessage(new InstantMessage() { FromUser = "vicepresident", Message = "test", ToUser = "dan" });
			messageInbox.TryAddMessage(new InstantMessage() { FromUser = "president", Message = "test", ToUser = "dan" });

			var messages = messageInbox.GetInstantMessagesByUser(inboxSearchParam);

			Assert.Equal(expectedCount, messages.Count);
		}
	}
}