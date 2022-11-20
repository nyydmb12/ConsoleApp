using Companion.Modules.Extensions.FinancialModule.POCOs;

namespace Companion.Modules.Extensions.FinancialModule.Providers
{
	public interface IFinancialProvider
	{
		string GetModulePostFix();
		Task<ITickerQuote> GetTickerPrice(string[] inputParams);
	}
}