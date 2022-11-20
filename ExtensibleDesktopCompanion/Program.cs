using ApplicationSettings;
using Companion.Modules;
using Companion.Modules.Extensions;
using Companion.Modules.Extensions.FinancialModule;
using Companion.Modules.Extensions.FinancialModule.Providers;
using Companion.Modules.Extensions.InstantMessaging;
using Companion.Modules.Extensions.InstantMessaging.Providers;

namespace ExtensibleDesktopCompanion
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var configurationData = GetConfigurationData();

			var handler = new SocketsHttpHandler
			{
				PooledConnectionLifetime = TimeSpan.FromMinutes(15) // Recreate every 15 minutes
			};
			var sharedClient = new HttpClient(handler);
			var moduleManager = new ModuleManager();
			moduleManager.AddModule(new FinancialModule(new IEXProvider(sharedClient)));
			moduleManager.AddModule(new FinancialModule(new AlphaAdvantageProvider(sharedClient)));
			moduleManager.AddModule(new InstantMessagingModule(new ServiceBusInstantMessagingProvider(configurationData), configurationData));
			string userInput = string.Empty;


			Console.WriteLine("Wecome to the Extensible Desktop Companion, enter \"help\" to see avaialable modules");

			while (userInput != "exit")
			{
				userInput = Console.ReadLine() ?? string.Empty;
				var userRequest = new UserRequest(userInput);
				moduleManager.ProcessUserRequest(userRequest);
			}
		}

		private static AppSettings GetConfigurationData()
		{
			var configHandler = new ConfigHandler<AppSettings>("appSettings.json");
			var appSettings = configHandler.GetConfigurationSection() ?? new AppSettings();

			if (string.IsNullOrWhiteSpace(appSettings?.AppData?.UserName))
			{
				appSettings.AppData!.UserName = ForceUserInput("Hello, Looks like we haven't met. Can I have your name for the messaging module?");
				configHandler.UpdateConfigFile(appSettings);
			}

			if (string.IsNullOrWhiteSpace(appSettings?.AppData?.AzureServiceBusConnectionString))
			{
				appSettings!.AppData!.AzureServiceBusConnectionString = ForceUserInput("Could you paste in the Azure Servicebus connection string Dan sent over?");
				configHandler.UpdateConfigFile(appSettings);
			}

			return appSettings;
		}

		private static string ForceUserInput(string prompt)
		{
			string userInput = string.Empty;
			while (string.IsNullOrWhiteSpace(userInput))
			{
				Console.WriteLine(prompt);
				userInput = Console.ReadLine() ?? string.Empty;
			}

			return userInput;
		}
	}
}