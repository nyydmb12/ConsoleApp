namespace Companion.Modules
{
	/// <summary>
	/// Contains a list of all available commands for a modual. 
	/// Also gets the command when called upon.
	/// </summary>
	public class AvailableCommands
	{
		private List<ICommand> _availableCommands = new List<ICommand>();

		public AvailableCommands()
		{

		}

		public void AddCommand(ICommand command)
		{
			_availableCommands.Add(command);
		}

		public void AddCommands(List<ICommand> commands)
		{
			_availableCommands.AddRange(commands);
		}

		/// <summary>
		/// Search for and return the requested command.
		/// </summary>
		/// <param name="commandText">the users command input</param>
		/// <returns>Return the command if found, otherwise return the NullCommand object.</returns>
		public ICommand GetCommand(string commandText)
		{
			var foundCommand = _availableCommands.FirstOrDefault(command => command.IsExecutionCommandMatch(commandText));
			return foundCommand != null ? foundCommand : new NullCommand(commandText);
		}

		public void PrintAvailableCommands()
		{
			Console.WriteLine($"These are the avaialable commands:");
			_availableCommands.ForEach(command => command.Print());
		}
	}
}
