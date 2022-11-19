namespace Companion.Modules
{
	/// <summary>
	/// This class will be used when notifying the user and other processes that a command 
	/// could not be found. 
	/// </summary>
	public class NullCommand : ICommand
	{
		private string _commandName;

		private string _description;

		public NullCommand(string commandName)
		{
			_commandName = commandName;
			_description = $"No commands the the name \"{_commandName}\" could be found.";
		}

		public bool IsExecutionCommandMatch(string commandText)
		{
			return false;
		}

		public void ExecuteCommand(string[] commandText)
		{
			Print();
		}

		public void Print()
		{
			Console.WriteLine($"{_commandName} - {_description}");
		}
	}
}
