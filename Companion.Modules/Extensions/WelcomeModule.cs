namespace Companion.Modules.Extensions
{
	/// <summary>
	/// This class introduces the help command, and will be inherited by more advanced modules.
	/// </summary>
	public class WelcomeModule : ModuleBase
	{
		public WelcomeModule(string moduleKeyword) : base(moduleKeyword)
		{
			_availableCommands.AddCommand(new Command(_helpCommand, "Print all commands and their descriptions.", (commandText) => _availableCommands.PrintAvailableCommands()));
		}

		public override bool ExecuteCommand(UserRequest userRequest)
		{
			var wasCommandExecuted = base.ExecuteCommand(userRequest);

			if (!wasCommandExecuted)
			{
				base.ExecuteCommand(new UserRequest() { CommandName = _helpCommand });
			}

			return wasCommandExecuted;
		}

	}
}