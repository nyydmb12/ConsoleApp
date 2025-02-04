﻿namespace Companion.Modules.Extensions
{
	/// <summary>
	/// The class will orchestrate commands with their corresponding modules
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
					Console.WriteLine($"These are the avaialable modules:");
					modules.ForEach(module => Console.WriteLine(module.GetModuleKeyword()));
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
				EventLogger.WriteErrorToEventLog($"Exception processing service user request {ex.Message} ");
				Console.WriteLine("Excetion Occured Processing Request");
			}
		}
	}
}