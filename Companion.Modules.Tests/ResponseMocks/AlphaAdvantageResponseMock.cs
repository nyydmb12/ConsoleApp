namespace Companion.Modules.Tests.ResponseMocks
{
	public static class AlphaAdvantageResponseMock
	{
		public static string JSONResponse { get; set; } = @"{
            ""Global Quote"": {
                ""01. symbol"": ""AAPL"",
                ""02. open"": ""152.3050"",
                ""03. high"": ""152.7000"",
                ""04. low"": ""149.9700"",
                ""05. price"": ""151.2900"",
                ""06. volume"": ""74829573"",
                ""07. latest trading day"": ""2022-11-18"",
                ""08. previous close"": ""150.7200"",
                ""09. change"": ""0.5700"",
                ""10. change percent"": ""0.3782%""
            }
        }";
	}
}
