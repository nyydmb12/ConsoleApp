using Companion.Modules.Extensions.FinancialModule.Providers;

namespace Companion.Modules.Extensions.FinancialModule
{
	/// <summary>
	/// This class introduces the fin command, and will interface with financial providers
	/// </summary>
	public class FinancialModule : WelcomeModule
	{
		private const string ModuleKeyword = "fin";
		public FinancialModule(IFinancialProvider financialProvider) : base($"{ModuleKeyword}-{financialProvider.GetModulePostFix()}")
		{
			_availableCommands.AddCommand(new Command("quote", "Gets the current price of a stock ticker. Example quote aapl ", (commandText) => financialProvider.GetTickerPrice(commandText)));
		}
	}
}