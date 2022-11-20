namespace Companion.Modules.Extensions
{
	/// <summary>
	/// This class introduces the help and exit command, and will be inherited by more advanced modules.
	/// </summary>
	public class WelcomeModule : ModuleBase
	{
		private const string _moduleKeyword = "app";
		public WelcomeModule() : base(_moduleKeyword)
		{
			AddWelcomeCommands();
		}

		public WelcomeModule(string moduleKeyword) : base(moduleKeyword)
		{
			AddWelcomeCommands();
		}

		private void AddWelcomeCommands()
		{
			_availableCommands.AddCommand(new Command(_helpCommand, "Print all commands and their descriptions.", (commandText) => _availableCommands.PrintAvailableCommands()));
			_availableCommands.AddCommand(new Command(_exitCommand, "Exit the program.", (commandText) => Environment.Exit(0)));
		}

		public override bool ExecuteCommand(UserRequest userRequest)
		{
			var wasCommandExecuted = base.ExecuteCommand(userRequest);
			// If the command wasn't executed, run the "help" command
			if (!wasCommandExecuted)
			{
				base.ExecuteCommand(new UserRequest() { CommandName = _helpCommand });
			}

			return wasCommandExecuted;
		}

	}
}