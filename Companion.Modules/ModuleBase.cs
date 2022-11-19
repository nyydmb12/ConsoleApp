namespace Companion.Modules
{
	/// <summary>
	/// This class will provide the barebone implementation for our console.
	/// </summary>
	public abstract class ModuleBase : IModuleBase
	{
		public static string _helpCommand = "help";

		private string ModuleKeyword;

		protected AvailableCommands _availableCommands = new AvailableCommands();

		public ModuleBase(string moduleKeyword)
		{
			ModuleKeyword = moduleKeyword;
		}

		public string GetModuleKeyword()
		{
			return ModuleKeyword;
		}

		/// <summary>
		/// Find and execute the user input command if found
		/// </summary>
		/// <param name="commandText">the users input</param>
		/// <returns>True if the command was found and executed, otherwise returns false.</returns>
		public virtual bool ExecuteCommand(UserRequest userRequest)
		{
			// Only pass in the module identifier
			var command = _availableCommands.GetCommand(userRequest.CommandName);
			command.ExecuteCommand(userRequest.CommandParameters);

			return command is Command;
		}
	}
}