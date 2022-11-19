using Companion.Modules;
using Companion.Modules.Extensions;
using Companion.Modules.Extensions.FinancialModule;
using Companion.Modules.Extensions.FinancialModule.Providers;

namespace ExtensibleDesktopCompanion
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var handler = new SocketsHttpHandler
			{
				PooledConnectionLifetime = TimeSpan.FromMinutes(15) // Recreate every 15 minutes
			};
			var sharedClient = new HttpClient(handler);
			var moduleManager = new ModuleManager();
			moduleManager.AddModule(new FinancialModule(new IEXProvider(sharedClient)));
			moduleManager.AddModule(new FinancialModule(new AlphaAdvantageProvider(sharedClient)));
			string userInput = string.Empty;


			Console.WriteLine("Wecome to the Extensible Desktop Companion, enter \"help\" to see avaialable modules");

			while (userInput != "exit")
			{
				userInput = Console.ReadLine() ?? string.Empty;
				var userRequest = new UserRequest(userInput);
				moduleManager.ProcessUserRequest(userRequest);
			}
		}
	}
}