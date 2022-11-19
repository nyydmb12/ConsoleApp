using Companion.Modules;
using Companion.Modules.Extensions;

namespace ExtensibleDesktopCompanion
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string userInput = string.Empty;
			while (userInput != "-exit")
			{
				var module = new WelcomeModule();
				Console.WriteLine("Wecome to the module, enter \"-help\" to see avaialable commands");
				userInput = Console.ReadLine() ?? string.Empty;
				module.ExecuteCommand(userInput);
			}
		}
	}
}