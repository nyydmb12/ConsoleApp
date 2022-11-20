using Companion.Modules.Extensions.FinancialModule.Providers;

namespace Companion.Modules.Extensions.FinancialModule
{
	/// <summary>
	/// This class introduces the fin command, and will interface with financial providers
	/// </summary>
	public class FinancialModule : WelcomeModule
	{
		private const string ModuleKeyword = "fin";
		private IFinancialProvider _financialProvider;
		public FinancialModule(IFinancialProvider financialProvider) : base($"{ModuleKeyword}-{financialProvider.GetModulePostFix()}")
		{
			_financialProvider = financialProvider;
			_availableCommands.AddCommand(new Command("quote", "Gets the current price of a stock ticker. Example quote aapl ", (commandParameters) => PrintTickerPrice(commandParameters)));
		}

		private void PrintTickerPrice(string[] commandParameters)
		{
			var tickerQuote = _financialProvider.GetTickerPrice(commandParameters).Result;
			// Only write to console if the ticker quote is valid. Otherwise the prvoider should have written a to the log. 
			if (tickerQuote.IsValid())
			{
				tickerQuote.WriteToConsole();
			}
		}
	}
}