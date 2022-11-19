namespace Companion.Modules
{
	public interface ICommand
	{
		void ExecuteCommand();
		bool IsExecutionCommandMatch(string commandText);
		void Print();
	}
}