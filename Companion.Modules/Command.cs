using System.ComponentModel;

namespace Companion.Modules
{
	/// <summary>
	/// This class will contain the name, description, and execution logic for all valid commands.
	/// </summary>
	public class Command : ICommand
	{
		private string _commandName;

		private string _description;

		private Action<string[]> _executionLogic;

		public Command(string commandName, string description, Action<string[]> executionLogic)
		{
			_commandName = commandName;
			_description = description;
			_executionLogic = executionLogic;
		}

		public bool IsExecutionCommandMatch(string commandText)
		{
			return string.Compare(_commandName, commandText, true) == 0;
		}

		public void ExecuteCommand(string[] commandText)
		{
			// Skip the first command since that is a module identifier
			_executionLogic(commandText.Skip(1).ToArray());
		}

		public void Print()
		{
			Console.WriteLine($"{_commandName} - {_description}");
		}
	}
}
