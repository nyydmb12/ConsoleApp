using System.Text.Json.Serialization;

namespace ApplicationSettings
{
	public class AppSettings
	{
		[JsonPropertyName("appData")]
		public AppData? AppData { get; set; }

		public AppSettings()
		{
			AppData = new AppData();
		}
	}
}
