using Companion.Modules.Extensions.FinancialModule.Providers;
using Companion.Modules.Tests.ResponseMocks;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;

namespace Companion.Modules.Tests
{
	public class FinancialProvider_Tests
	{
		[Fact]
		public async Task ShouldReturnTickerPriceIEX()
		{
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(uri => uri.RequestUri.ToString().Contains("aapl")), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(IEXResponseMock.JSONResponse)

				});
			var client = new HttpClient(mockHttpMessageHandler.Object);
			var iexProvider = new IEXProvider(client);
			var response = await iexProvider.GetTickerPrice(new string[1] { "aapl" });

			Assert.True(response.IsValid());
		}

		[Fact]
		public async Task ShouldReturnTickerPriceAlpha()
		{
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(uri => uri.RequestUri.ToString().Contains("aapl")), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(AlphaAdvantageResponseMock.JSONResponse)

				});
			var client = new HttpClient(mockHttpMessageHandler.Object);
			var alphaAdvantageProvider = new AlphaAdvantageProvider(client);
			var response = await alphaAdvantageProvider.GetTickerPrice(new string[1] { "aapl" });

			Assert.True(response.IsValid());
		}

		[Fact]
		public async Task ShouldNotReturnTickerPriceAlpha()
		{
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(uri => uri.RequestUri.ToString().Contains("N/A")), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(InvalidAlphaAdvantageResponseMock.JSONResponse)

				});
			var client = new HttpClient(mockHttpMessageHandler.Object);
			var alphaAdvantageProvider = new AlphaAdvantageProvider(client);
			var response = await alphaAdvantageProvider.GetTickerPrice(new string[1] { "N/A" });

			Assert.False(response.IsValid());
		}

		[Fact]
		public async Task ShouldNotReturnTickerPriceIEX()
		{
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(uri => uri.RequestUri.ToString().Contains("N/A")), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(InvalidIEXResopnseMock.JSONResponse)

				});
			var client = new HttpClient(mockHttpMessageHandler.Object);
			var iexProvider = new IEXProvider(client);
			var response = await iexProvider.GetTickerPrice(new string[1] { "N/A" });

			Assert.False(response.IsValid());
		}
	}
}