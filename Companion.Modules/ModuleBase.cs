namespace Companion.Modules
{
	/// <summary>
	/// This class will provide the barebone implementation for our console.
	/// </summary>
	public abstract class ModuleBase
	{
		protected AvailableCommands _availableCommands = new AvailableCommands();

		/// <summary>
		/// Find and execute the user input command if found
		/// </summary>
		/// <param name="commandText">the users input</param>
		/// <returns>True if the command was found and executed, otherwise returns false.</returns>
		public virtual bool ExecuteCommand(string commandText)
		{
			var command = _availableCommands.GetCommand(commandText);
			command.ExecuteCommand();

			return command is Command;
		}




	}
}