using System.Text.Json.Serialization;

namespace Companion.Modules.Extensions.FinancialModule.POCOs
{
	public class AlphaAdvantageQuote
	{
		private string? CompanyName { get; set; }

		private string? ChangePercent { get; set; }

		private string? LastestTradingDay { get; set; }

		private string? LatestPrice { get; set; }

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
	}
}
