using ApplicationSettings;
using Companion.Modules;
using Companion.Modules.Extensions;
using Companion.Modules.Extensions.FinancialModule;
using Companion.Modules.Extensions.FinancialModule.Providers;
using Companion.Modules.Extensions.InstantMessaging;
using Companion.Modules.Extensions.InstantMessaging.POCOs;
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
				PooledConnectionLifetime = TimeSpan.FromMinutes(15)
			};
			var sharedClient = new HttpClient(handler);
			var messageInbox = new MessageInbox();
			var moduleManager = new ModuleManager();
			// Provides basic commands like help and exit
			moduleManager.AddModule(new WelcomeModule());
			// Adds stock quote look up from IEX exchange
			moduleManager.AddModule(new FinancialModule(new IEXProvider(sharedClient)));
			// Adds stock quote look up from Alpha Advantage
			moduleManager.AddModule(new FinancialModule(new AlphaAdvantageProvider(sharedClient)));
			// Adds message sending and receiving to other users
			moduleManager.AddModule(new InstantMessagingModule(ServiceBusInstantMessagingProvider.CreateAsync(configurationData, messageInbox).Result, messageInbox, configurationData));

			string userInput;

			Console.WriteLine("Wecome to the Extensible Desktop Companion, enter \"help\" to see avaialable modules.");
			Console.WriteLine("Enter \"help\" after the module name to see the avialable commands.");

			// Loop until the app exit command fires
			while (true)
			{
				userInput = Console.ReadLine() ?? string.Empty;
				var userRequest = new UserRequest(userInput);

				moduleManager.ProcessUserRequest(userRequest);
			}
		}
		/// <summary>
		/// Load the config file and get missing data from the user.
		/// Update the file with the new data.
		/// </summary>
		private static AppSettings GetConfigurationData()
		{
			var configHandler = new ConfigHandler<AppSettings>("appSettings.json");
			var appSettings = configHandler.GetConfigurationSection() ?? new AppSettings();

			if (string.IsNullOrWhiteSpace(appSettings?.AppData?.UserName))
			{
				appSettings!.AppData!.UserName = ForceUserInput("Hello, Looks like we haven't met. Can I have your name for the messaging module?");
				configHandler.UpdateConfigFile(appSettings);
			}

			if (string.IsNullOrWhiteSpace(appSettings?.AppData?.AzureServiceBusConnectionString))
			{
				appSettings!.AppData!.AzureServiceBusConnectionString = ForceUserInput("Could you paste in the Azure Servicebus connection string Dan sent over?");
				configHandler.UpdateConfigFile(appSettings);
			}

			return appSettings;
		}

		/// <summary>
		/// Keep asking the user for input unless they enter skip. This however will cause messaging not to work.
		/// </summary>
		private static string ForceUserInput(string prompt)
		{
			string userInput = string.Empty;
			while (string.IsNullOrWhiteSpace(userInput) && userInput != "skip")
			{
				Console.WriteLine(prompt);
				Console.WriteLine("You can also enter skip and manually update the appSettings.json file with this value.");
				userInput = Console.ReadLine() ?? string.Empty;
			}

			return userInput;
		}
	}
}