namespace Companion.Modules.Extensions.FinancialModule.POCOs
{
	/// <summary>
	/// This class is used for working within our system
	/// </summary>
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