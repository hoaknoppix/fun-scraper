namespace tests;

using System.Net;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Moq;
using Moq.Contrib.HttpClient;
using Moq.Protected;
using Xunit;

public class VnExpressScrapServiceTest
{
    [Fact]
    public async void EmptyContent()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .SetupRequest(
                HttpMethod.Get,
                "https://alpha3-api3.vnexpress.net/api/mobile?type=homev5&app_id=9e304d&offset=0"
            )
            .ReturnsResponse("{\"data\":[]}");
        var httpClient = mockHandler.CreateClient();
        VnExpressScrapService vnExpressScrapService = new VnExpressScrapService(httpClient);
        var result = await vnExpressScrapService.GetContent();
        Assert.Equal(0, result.Count);
    }

    [Fact]
    public async void ValidContent()
    {
        HttpClient httpClient = MockHttpClient();
        VnExpressScrapService vnExpressScrapService = new VnExpressScrapService(httpClient);
        var result = await vnExpressScrapService.GetContent();
        Assert.Equal(1, result.Count);
        Assert.Equal(5, result[0].like);
        Assert.Equal("Sample Title", result[0].title);
        Assert.Equal("https://vnexpress.net/sample_url.html", result[0].url);
    }

    private static HttpClient MockHttpClient()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var projectRootDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        var filePath = Path.Combine(projectRootDirectory, "TestData", "dummy_vnexpress.json");
        var fileContent = File.ReadAllText(filePath);
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .SetupRequest(
                HttpMethod.Get,
                "https://alpha3-api3.vnexpress.net/api/mobile?type=homev5&app_id=9e304d&offset=0"
            )
            .ReturnsResponse(fileContent);
        filePath = Path.Combine(projectRootDirectory, "TestData", "dummy_vnexpress_comments.json");
        fileContent = File.ReadAllText(filePath);
        mockHandler
            .SetupRequest(
                HttpMethod.Get,
                "https://usi-saas.vnexpress.net/index/get?offset=0&frommobile=0&sort_by=like&is_onload=0&objectid=4838014&objecttype=1&siteid=1002565"
            )
            .ReturnsResponse(fileContent);
        var httpClient = mockHandler.CreateClient();
        return httpClient;
    }
}
