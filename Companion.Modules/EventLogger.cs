using System.Diagnostics;

namespace Companion.Modules
{
	public class EventLogger
	{
		public static void WriteErrorToEventLog(string message)
		{
			WriteToEventLog(message, EventLogEntryType.Error);
		}

		public static void WriteInfoToEventLog(string message)
		{
			WriteToEventLog(message, EventLogEntryType.Information);
		}

		private static void WriteToEventLog(string message, EventLogEntryType eventLogEntryType )
		{
			using (EventLog eventLog = new EventLog("Application"))
			{
				eventLog.Source = "Application";
				eventLog.WriteEntry(message, eventLogEntryType, 1, 1);
			}
		}
	}
}
