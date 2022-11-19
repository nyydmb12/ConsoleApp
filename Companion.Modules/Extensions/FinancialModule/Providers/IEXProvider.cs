using Companion.Modules.Extensions.FinancialModule.DTOs;
using System.Text.Json;

namespace Companion.Modules.Extensions.FinancialModule.Providers
{
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

		public async void GetTickerPrice(string[] inputParams)
		{
			try
			{
				if (inputParams.Any())
				{
					var tickerName = inputParams[0];
					await using Stream stream = await _httpClient.GetStreamAsync(string.Format(tickerQuoteEndpont, tickerName, iexToken));
					using var streamReader = new StreamReader(stream);
					var iexTickerQuote = JsonSerializer.Deserialize<IEXTickerQuote[]>(stream);

					if (iexTickerQuote != null && !string.IsNullOrWhiteSpace(iexTickerQuote[0].CompanyName))
					{
						new POCOs.IEXTickerQuote(iexTickerQuote[0]).WriteToConsole();
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
				Console.WriteLine("Exception occured processing IEX request");
			}
		}
	}
}
