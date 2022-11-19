using Companion.Modules.Extensions.FinancialModule.DTOs;
using System.Text.Json;

namespace Companion.Modules.Extensions.FinancialModule.Providers
{
	public class IEXProvider
	{
		// This is public key with limited permissions
		private static readonly string iexToken = "pk_569b8d8f0ccb434fa353691cb3fd2c17";
		private static readonly string  tickerQuoteEndpont = "https://api.iex.cloud/v1/data/core/quote/{0}?token={1}";

		private HttpClient _httpClient;

		public IEXProvider(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async void GetTickerPrice(string[] ticker)
		{
			await using Stream stream = await _httpClient.GetStreamAsync(string.Format(tickerQuoteEndpont, ticker, iexToken));
			var iexTickerQuote = await JsonSerializer.DeserializeAsync<IEXTickerQuote>(stream);
		
		}

	}
}
