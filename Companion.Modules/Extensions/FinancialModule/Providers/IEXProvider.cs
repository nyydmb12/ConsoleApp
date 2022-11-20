using Companion.Modules.Extensions.FinancialModule.POCOs;
using System.Text.Json;

namespace Companion.Modules.Extensions.FinancialModule.Providers
{
	/// <summary>
	/// This class with communicate with IEX to get stock quotes. Trial ends in 1 month!
	/// </summary>
	public class IEXProvider : IFinancialProvider
	{
		// This is public key with limited permissions
		private readonly string iexToken = "pk_569b8d8f0ccb434fa353691cb3fd2c17";
		private readonly string tickerQuoteEndpont = "https://api.iex.cloud/v1/data/core/quote/{0}?token={1}";
		// Add a postfix to the module name to demo multiple providers.
		private const string modulePostFix = "iex";

		private HttpClient _httpClient;

		public IEXProvider(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public string GetModulePostFix()
		{
			return modulePostFix;
		}

		public async Task<ITickerQuote> GetTickerPrice(string[] inputParams)
		{
			var tickerQuote = new POCOs.IEXTickerQuote();
			try
			{
				if (inputParams.Any())
				{
					var tickerName = inputParams[0];
					await using Stream stream = await _httpClient.GetStreamAsync(string.Format(tickerQuoteEndpont, tickerName, iexToken));
					using var streamReader = new StreamReader(stream);
					var iexTickerQuote = JsonSerializer.Deserialize<DTOs.IEXTickerQuote[]>(stream);

					if (iexTickerQuote != null && !string.IsNullOrWhiteSpace(iexTickerQuote[0].CompanyName))
					{
						tickerQuote = new POCOs.IEXTickerQuote(iexTickerQuote[0]);
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
				EventLogger.WriteErrorToEventLog($"Exception occured looking up ticker {ex.Message} ");
				Console.WriteLine("Exception occured processing IEX request");
			}

			return tickerQuote;
		}
	}
}
