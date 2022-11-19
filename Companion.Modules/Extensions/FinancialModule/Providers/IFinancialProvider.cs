namespace Companion.Modules.Extensions.FinancialModule.Providers
{
	public interface IFinancialProvider
	{
		string GetModulePostFix();
		void GetTickerPrice(string[] inputParams);
	}
}