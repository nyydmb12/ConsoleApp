using System.Text.Json.Serialization;

namespace ApplicationSettings
{
	public class AppData
	{
		[JsonPropertyName("userName")]
		public string? UserName { get; set; }


		[JsonPropertyName("azureServiceBusConnectionString")]
		public string? AzureServiceBusConnectionString { get; set; }
	}
}
