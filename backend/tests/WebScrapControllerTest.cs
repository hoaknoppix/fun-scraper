namespace tests;

using System.Net;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Moq;
using Moq.Contrib.HttpClient;
using Moq.Protected;
using Xunit;
using WebScrapApi.Controllers;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class WebScrapControllerTest
{
    [Fact]
    public async Task GetTuoiTreValidContent()
    {
        var mockHttpClient = new Mock<HttpClient>();
        var mockTuoiTreService = new Mock<TuoiTreScrapService>(mockHttpClient.Object);
        mockTuoiTreService.Setup(service => service.GetContent()).ReturnsAsync(new List<Content>());
        var mockVnExpressService = new Mock<VnExpressScrapService>(mockHttpClient.Object);
        // mockVnExpressService.Setup(mockVnExpressService => mockVnExpressService.GetContent()).ReturnsAsync(new List<Content>());
        WebScrapController webScrapController = new WebScrapController(
            mockVnExpressService.Object,
            mockTuoiTreService.Object
        );
        var result = await webScrapController.GetTuoiTre();
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task GetTuoiTreServerError()
    {
        var mockHttpClient = new Mock<HttpClient>();
        var mockTuoiTreService = new Mock<TuoiTreScrapService>(mockHttpClient.Object);
        mockTuoiTreService
            .Setup(service => service.GetContent())
            .ThrowsAsync(new HttpRequestException("A exception"));
        var mockVnExpressService = new Mock<VnExpressScrapService>(mockHttpClient.Object);
        WebScrapController webScrapController = new WebScrapController(
            mockVnExpressService.Object,
            mockTuoiTreService.Object
        );
        var result = await webScrapController.GetTuoiTre();
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetVnExpressValidContent()
    {
        var mockHttpClient = new Mock<HttpClient>();
        var mockVnExpressService = new Mock<VnExpressScrapService>(mockHttpClient.Object);
        mockVnExpressService.Setup(service => service.GetContent()).ReturnsAsync(new List<Content>());
        var mockTuoiTreService = new Mock<TuoiTreScrapService>(mockHttpClient.Object);
        WebScrapController webScrapController = new WebScrapController(
            mockVnExpressService.Object,
            mockTuoiTreService.Object
        );
        var result = await webScrapController.GetVnExpress();
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task GetVnExpressServerError()
    {
        var mockHttpClient = new Mock<HttpClient>();
        var mockVnExpressService = new Mock<VnExpressScrapService>(mockHttpClient.Object);
        mockVnExpressService
            .Setup(service => service.GetContent())
            .ThrowsAsync(new HttpRequestException("A exception"));
        var mockTuoiTreService = new Mock<TuoiTreScrapService>(mockHttpClient.Object);
        WebScrapController webScrapController = new WebScrapController(
            mockVnExpressService.Object,
            mockTuoiTreService.Object
        );
        var result = await webScrapController.GetVnExpress();
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetVnExpressJSONError()
    {
        var mockHttpClient = new Mock<HttpClient>();
        var mockVnExpressService = new Mock<VnExpressScrapService>(mockHttpClient.Object);
        mockVnExpressService
            .Setup(service => service.GetContent())
            .ThrowsAsync(new JsonException("A exception"));
        var mockTuoiTreService = new Mock<TuoiTreScrapService>(mockHttpClient.Object);
        WebScrapController webScrapController = new WebScrapController(
            mockVnExpressService.Object,
            mockTuoiTreService.Object
        );
        var result = await webScrapController.GetVnExpress();
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}
