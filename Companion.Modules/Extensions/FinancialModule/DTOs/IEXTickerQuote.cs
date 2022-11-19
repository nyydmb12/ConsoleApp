using System.Text.Json.Serialization;

namespace Companion.Modules.Extensions.FinancialModule.DTOs
{
	public class IEXTickerQuote
	{
		[JsonPropertyName("companyName")]
		public string? CompanyName { get; set; }

		[JsonPropertyName("changePercent")]
		public double? changePercent { get; set; }

		[JsonPropertyName("highSource")]
		public string? Delay { get; set; }

		[JsonPropertyName("latestPrice")]
		public string? LatestPrice { get; set; }
	}
}
