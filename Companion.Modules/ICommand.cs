namespace Companion.Modules
{
	public interface ICommand
	{
		void ExecuteCommand(string[] commandParameters);
		bool IsExecutionCommandMatch(string commandText);
		void Print();
	}
}