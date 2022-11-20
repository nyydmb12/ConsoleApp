using System.IO;
using System.Text.Json;

namespace ApplicationSettings
{
	/// <summary>
	/// This class will read from and update the config file. Since I'm trying to make it so the .config doesn't need to be manually updated, 
	/// I won't use the IConfiguration libraries
	/// </summary>
	public class ConfigHandler<T>
	{
		private string _configFileName;

		private string _configFilePath;
		public ConfigHandler(string configFileName)
		{
			_configFileName = configFileName;
			_configFilePath = $"{Directory.GetCurrentDirectory()}\\{_configFileName}";
		}

		public T? GetConfigurationSection()
		{
			T? appSettings = default(T);
			try
			{
				using var streamReader = new StreamReader(_configFilePath);
				var jsonDocument = JsonDocument.Parse(streamReader.ReadToEnd());
				appSettings = jsonDocument.Deserialize<T>();

			}
			catch (Exception ex)
			{
				Console.WriteLine("Could not load configuration");
			}
			return appSettings;
		}

		public void UpdateConfigFile(T configurationObject)
		{
			using var streamWriter = new StreamWriter(_configFilePath);
			streamWriter.WriteAsync(JsonSerializer.Serialize(configurationObject));

		}
	}
}