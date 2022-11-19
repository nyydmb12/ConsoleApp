namespace Companion.Modules.Extensions
{
	/// <summary>
	/// This class introduces the help command, and will be inherited by more advanced modules.
	/// </summary>
	public class ModuleManager
	{
		private List<IModuleBase> modules = new List<IModuleBase>();

		public void AddModule(IModuleBase module)
		{
			modules.Add(module);
		}

		public void ProcessUserRequest(UserRequest userRequest)
		{
			try
			{
				if (userRequest.ModuleName == ModuleBase._helpCommand)
				{
					modules.ForEach(module => Console.WriteLine($"These are the avaialable modules: {module.GetModuleKeyword()}"));
				}
				else
				{
					var moduleToExecute = modules.Where(module => string.Compare(module.GetModuleKeyword(), userRequest.ModuleName, true) == 0).FirstOrDefault();

					if (moduleToExecute != null)
					{
						moduleToExecute.ExecuteCommand(userRequest);
					}
					else
					{
						Console.WriteLine($"Module \"{userRequest.ModuleName}\" was not found.");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Excetion Occured Processing Request");
			}
		}
	}
}