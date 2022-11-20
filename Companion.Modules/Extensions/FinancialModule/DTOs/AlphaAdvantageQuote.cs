using System.Text.Json.Serialization;

namespace Companion.Modules.Extensions.FinancialModule.DTOs
{
	/// <summary>
	/// This class is only for communicating with Alpha Advantage
	/// </summary>
	public class AlphaAdvantageQuote
	{
		[JsonPropertyName("01. symbol")]
		public string? Symbol { get; set; }

		[JsonPropertyName("10. change percent")]
		public string? ChangePercent { get; set; }

		[JsonPropertyName("07. latest trading day")]
		public string? LastestTradingDay { get; set; }

		[JsonPropertyName("05. price")]
		public string? LatestPrice { get; set; }

	}
}
