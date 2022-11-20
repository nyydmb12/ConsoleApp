namespace Companion.Modules.Extensions.InstantMessaging.DTOs
{
	public interface IInstantMessage
	{
		string? FromUser { get; set; }
		string? Message { get; set; }
		string? ToUser { get; set; }
	}
}