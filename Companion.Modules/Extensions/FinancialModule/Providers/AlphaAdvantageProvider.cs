using Companion.Modules.Extensions.FinancialModule.POCOs;
using System.Text.Json;

namespace Companion.Modules.Extensions.FinancialModule.Providers
{
	/// <summary>
	/// This api is rate limited at 5 requests per miniute.
	/// </summary>
	public class AlphaAdvantageProvider : IFinancialProvider
	{
		private readonly string iexToken = "THVF7H4YU21YE76S";
		private readonly string tickerQuoteEndpont = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={0}&apikey={1}";
		private const string modulePostFix = "alpha";

		private HttpClient _httpClient;

		public AlphaAdvantageProvider(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public string GetModulePostFix()
		{
			return modulePostFix;
		}

		public async Task<ITickerQuote> GetTickerPrice(string[] inputParams)
		{
			var tickerQuote = new POCOs.AlphaAdvantageQuote();
			try
			{
				if (inputParams.Any())
				{
					var tickerName = inputParams[0];
					await using Stream stream = await _httpClient.GetStreamAsync(string.Format(tickerQuoteEndpont, tickerName, iexToken));
					using var streamReader = new StreamReader(stream);
					var jsonDocument = JsonDocument.Parse(await streamReader.ReadToEndAsync());
					var wrapperObject = jsonDocument.RootElement.GetProperty("Global Quote");
					var alphaAdvantageQuote = wrapperObject.Deserialize<DTOs.AlphaAdvantageQuote>();

					if (alphaAdvantageQuote != null && !string.IsNullOrWhiteSpace(alphaAdvantageQuote?.Symbol))
					{
						tickerQuote = new POCOs.AlphaAdvantageQuote(alphaAdvantageQuote);
					}
					else
					{
						Console.WriteLine("Ticker was not found, check the ticker name");
					}
				}
				else
				{
					Console.WriteLine("No parameters were passed into GetTickerPrice");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception occured processing Alpha Advantage request");
			}

			return tickerQuote;
		}

	}
}
