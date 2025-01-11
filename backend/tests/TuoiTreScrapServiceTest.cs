namespace tests;

using System.Net;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Moq;
using Moq.Contrib.HttpClient;
using Moq.Protected;
using Xunit;

public class TuoiTreScrapServiceTest
{
    [Fact]
    public async Task EmptyContent()
    {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .SetupRequest(HttpMethod.Get, "https://tuoitre.vn/tin-moi-nhat.htm")
            .ReturnsResponse("");
        var httpClient = mockHandler.CreateClient();
        TuoiTreScrapService tuoiTreScrapService = new TuoiTreScrapService(httpClient);
        var result = await tuoiTreScrapService.GetContent();
        Assert.Empty(result);
    }

    [Fact]
    public async Task ValidContent()
    {
        HttpClient httpClient = MockHttpClient();
        TuoiTreScrapService tuoiTreScrapService = new TuoiTreScrapService(httpClient);
        var result = await tuoiTreScrapService.GetContent();
        Assert.Single(result);
        Assert.Equal(56, result[0].like);
        Assert.Equal("Sample title", result[0].title);
        Assert.Equal(
            "https://tuoitre.vn/hlv-kim-sang-sik-3-4-thang-roi-toi-khong-duoc-gap-gia-dinh-20250108232827683.htm",
            result[0].url
        );
    }

    private static HttpClient MockHttpClient()
    {
        var currentDirectory = Directory.GetCurrentDirectory() ?? "";
        var projectRootDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName ?? "";
        var filePath = Path.Combine(projectRootDirectory, "TestData", "dummy_tuoitre.html");
        var fileContent = File.ReadAllText(filePath);
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
            .SetupRequest(HttpMethod.Get, "https://tuoitre.vn/tin-moi-nhat.htm")
            .ReturnsResponse(fileContent);
        filePath = Path.Combine(projectRootDirectory, "TestData", "dummy_tuoitre_comments.html");
        fileContent = File.ReadAllText(filePath);
        mockHandler
            .SetupRequest(
                HttpMethod.Get,
                "https://tuoitre.vn/hlv-kim-sang-sik-3-4-thang-roi-toi-khong-duoc-gap-gia-dinh-20250108232827683.htm"
            )
            .ReturnsResponse(fileContent);
        var httpClient = mockHandler.CreateClient();
        return httpClient;
    }
}
