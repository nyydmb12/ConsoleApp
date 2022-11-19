using Companion.Modules;
using Companion.Modules.Extensions;

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

			string userInput = string.Empty;
			while (userInput != "-exit")
			{
				var module = new WelcomeModule();
				Console.WriteLine("Wecome to the module, enter \"-help\" to see avaialable commands");
				userInput = Console.ReadLine() ?? string.Empty;
				module.ExecuteCommand(userInput.Split(" "));
			}
		}
	}
}