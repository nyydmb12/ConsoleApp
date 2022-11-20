namespace Companion.Modules.Extensions.FinancialModule.POCOs
{
	public interface ITickerQuote
	{
		string? ChangePercent { get; }
		string? CompanyName { get; }
		string? LastestTradingDay { get; }
		string? LatestPrice { get; }

		void WriteToConsole();

		bool IsValid();
	}
}