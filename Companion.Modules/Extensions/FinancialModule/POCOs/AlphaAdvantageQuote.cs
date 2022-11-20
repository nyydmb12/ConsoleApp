using System.Text.Json.Serialization;

namespace Companion.Modules.Extensions.FinancialModule.POCOs
{
	public class AlphaAdvantageQuote : ITickerQuote
	{
		public string? CompanyName { get; }

		public string? ChangePercent { get; }

		public string? LastestTradingDay { get; }

		public string? LatestPrice { get; }

		public AlphaAdvantageQuote() { }

		public AlphaAdvantageQuote(DTOs.AlphaAdvantageQuote AlphaAdvantageQuote)
		{
			CompanyName = AlphaAdvantageQuote.Symbol;
			ChangePercent = AlphaAdvantageQuote.ChangePercent;
			LastestTradingDay = AlphaAdvantageQuote.LastestTradingDay;
			LatestPrice = AlphaAdvantageQuote.LatestPrice;
		}

		public void WriteToConsole()
		{
			Console.WriteLine($"Symbol Name: {CompanyName}");
			Console.WriteLine($"Change Percent: {ChangePercent}");
			Console.WriteLine($"Price Delay: {LastestTradingDay}");
			Console.WriteLine($"Latest Price: {LatestPrice}");
		}

		public bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(CompanyName);
		}
	}
}
