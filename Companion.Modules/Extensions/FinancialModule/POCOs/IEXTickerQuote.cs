using System.Text.Json.Serialization;

namespace Companion.Modules.Extensions.FinancialModule.POCOs
{
	public class IEXTickerQuote
	{
		private string? CompanyName { get; set; }

		private double? ChangePercent { get; set; }

		private string? Delay { get; set; }

		private double? LatestPrice { get; set; }

		public IEXTickerQuote(DTOs.IEXTickerQuote dtoIEXTickerQuote)
		{
			CompanyName = dtoIEXTickerQuote.CompanyName;
			ChangePercent = dtoIEXTickerQuote.changePercent;
			Delay = dtoIEXTickerQuote.Delay;
			LatestPrice = dtoIEXTickerQuote.LatestPrice;
		}

		public void WriteToConsole()
		{
			Console.WriteLine($"Company Name: {CompanyName}");
			Console.WriteLine($"Change Percent: {ChangePercent}");
			Console.WriteLine($"Price Delay: {Delay}");
			Console.WriteLine($"Latest Price: {LatestPrice}");
		}
	}
}
