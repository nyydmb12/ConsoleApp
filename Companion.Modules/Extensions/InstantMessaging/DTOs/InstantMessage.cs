namespace Companion.Modules.Extensions.InstantMessaging.DTOs
{
	public class InstantMessage : IInstantMessage
	{
		public string? FromUser { get; set; }
		public string? ToUser { get; set; }
		public string? Message { get; set; }
	}
}
