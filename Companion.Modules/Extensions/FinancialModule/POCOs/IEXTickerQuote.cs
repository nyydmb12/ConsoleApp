using System.Text.Json.Serialization;

namespace Companion.Modules.Extensions.FinancialModule.POCOs
{
	public class IEXTickerQuote : ITickerQuote
	{
		public string? CompanyName { get; }

		public string? ChangePercent { get; }

		public string? LastestTradingDay { get; }

		public string? LatestPrice { get; }

		public IEXTickerQuote() { }

		public IEXTickerQuote(DTOs.IEXTickerQuote dtoIEXTickerQuote)
		{
			CompanyName = dtoIEXTickerQuote.CompanyName;
			ChangePercent = dtoIEXTickerQuote.changePercent.ToString();
			LastestTradingDay = dtoIEXTickerQuote.Delay;
			LatestPrice = dtoIEXTickerQuote.LatestPrice.ToString();
		}

		public void WriteToConsole()
		{
			Console.WriteLine($"Company Name: {CompanyName}");
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
