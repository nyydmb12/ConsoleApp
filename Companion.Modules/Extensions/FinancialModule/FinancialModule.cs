using Companion.Modules.Extensions.FinancialModule.Providers;

namespace Companion.Modules.Extensions.FinancialModule
{
	/// <summary>
	/// This class introduces the help command, and will be inherited by more advanced modules.
	/// </summary>
	public class FinancialModule : WelcomeModule
	{
		private const string ModuleKeyword = "fin";
		public FinancialModule(IFinancialProvider iexProvider) : base($"{ModuleKeyword}-{iexProvider.GetModulePostFix()}")
		{
			_availableCommands.AddCommand(new Command("quote", "Gets the current price of a stock ticker. Example quote aapl ", (commandText) => iexProvider.GetTickerPrice(commandText)));
		}

		public override bool ExecuteCommand(UserRequest userRequest)
		{
			var wasCommandExecuted = base.ExecuteCommand(userRequest);

			return wasCommandExecuted;
		}
	}
}