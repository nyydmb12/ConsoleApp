namespace Companion.Modules
{
	public interface IModuleBase
	{
		string GetModuleKeyword();

		bool ExecuteCommand(UserRequest userRequest);
	}
}