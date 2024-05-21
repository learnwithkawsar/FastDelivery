using FastDelivery.Framework.Core.Pagination;
using MongoDB.Bson;
using System.Net.Http.Headers;

namespace OrderService.IntegrationTests.Api;

public class ParcelControllerTests : IClassFixture<WebAppFactory<Program>>
{
    private readonly Mock<IParcelRepository> _parcelRepositoryMock;
    private readonly HttpClient _client;
    private readonly WebAppFactory<Program> _factory;
    AddParcelCommand? request = null;

    public ParcelControllerTests(WebAppFactory<Program> webApplicationFactory)
    {
        _parcelRepositoryMock = new();
        string token = TokenGenerator.GenerateJwtToken();

        _factory = webApplicationFactory;
        _client = webApplicationFactory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var rand = new Random();
        string invoiceId = rand.Next(1000000, 99999999).ToString();
        request = new AddParcelCommand(1, invoiceId, "Test Name", "01475201214", "address", 50M, "note");
    }

    [Fact]
    public async Task AddParcel_ValidRequest_ReturnsSuccess()
    {
        // Arrange    
        _factory.MockEventBus.Setup(eb => eb.PublishAsync(It.IsAny<IntegrationEvent>())).Returns(Task.CompletedTask);
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/orders", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299               
        string responseContent = await response.Content.ReadAsStringAsync();
        var parcelDto = JsonConvert.DeserializeObject<ParcelDto>(responseContent);

        parcelDto.Should().NotBeNull();
        // Verify the IEventBus was called
        _factory.MockEventBus.Verify(eb => eb.PublishAsync(It.IsAny<IntegrationEvent>()), Times.AtLeast(1));

    }

    [Fact]
    public async Task AddAsync_Valid_AddParcelCommandHandler_Request()
    {
        // Arrange   
        var command = new AddParcelCommand(1, "123456", "John Doe", "1234567890", "123 Main St", 50.00m, "Test note");
        var handler = new AddParcelCommandHandler(_parcelRepositoryMock.Object);

        //Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
    }
    [Fact]
    public async Task AddAsync_InvalidInvoiceId_AddParcelCommandHandler_Request()
    {
        // Arrange   
        var command = new AddParcelCommand(1, "", "John Doe", "1234567890", "123 Main St", 50.00m, "Test note");
        var handler = new AddParcelCommandHandler(_parcelRepositoryMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(command, default));
    }

    [Fact]
    public async Task SearchAsync_ReturnsPagedListOfParcels()
    {
        // Arrange
        var parameters = new ParcelParametersDto
        {
            // Set up your test parameters here
            //InvoiceId = "123456",
            PageNumber = 1,
            PageSize = 10
        };

        var content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync($"/api/v1/orders/search", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        string responseString = await response.Content.ReadAsStringAsync();
        var parcels = PagedList<ParcelDto>.FromJson(responseString);

        Assert.NotNull(parcels);
        Assert.True(parcels.Data.Count > 0); // Adjust this assertion based on expected results
                                             //await Verify

        await Verify(parcels);
    }

}
