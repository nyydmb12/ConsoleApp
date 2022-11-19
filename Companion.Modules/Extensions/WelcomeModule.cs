namespace Companion.Modules.Extensions
{
	/// <summary>
	/// This class introduces the help command, and will be inherited by more advanced modules.
	/// </summary>
	public class WelcomeModule : ModuleBase
	{
		private static string _helpCommand = "-help";

		public WelcomeModule()
		{
			_availableCommands.AddCommand(new Command(_helpCommand, "Print all commands and their descriptions.", () => _availableCommands.PrintAvailableCommands()));
		}

		public override bool ExecuteCommand(string commandText)
		{
			var wasCommandExecuted = base.ExecuteCommand(commandText);

			if (!wasCommandExecuted)
			{
				base.ExecuteCommand(_helpCommand);
			}

			return wasCommandExecuted;
		}

	}
}