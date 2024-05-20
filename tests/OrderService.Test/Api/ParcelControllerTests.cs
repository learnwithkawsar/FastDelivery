


using System.Net.Http.Headers;

namespace OrderService.IntegrationTests.Api;

public class ParcelControllerTests : IClassFixture<WebAppFactory<Program>>
{
    private readonly Mock<IParcelRepository> _parcelRepositoryMock;
    private readonly Mock<IValidator<ParcelInfo>> _iVaildator;
    private readonly HttpClient _client;
    private readonly WebAppFactory<Program> _factory;

    public ParcelControllerTests(WebAppFactory<Program> webApplicationFactory)
    {
        _parcelRepositoryMock = new();
        _iVaildator = new();
        string token = TokenGenerator.GenerateJwtToken();

        _factory = webApplicationFactory;
        _client = webApplicationFactory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [Theory]
    [InlineData("123456", "John Doe", "1234567890", "123 Main St", 50.00, "Test note 1")]
    [InlineData("789101", "Jane Smith", "0987654321", "456 Elm St", 75.00, "Test note 2")]
    [InlineData("112131", "Alice Brown", "5647382910", "789 Oak St", 100.00, "Test note 3")]

    public async Task AddParcel_ValidRequest_ReturnsSuccess(
         string invoiceId,
        string fullName,
        string mobileNo,
        string address,
        decimal codAmount,
        string note)
    {
        // Arrange    
        _factory.MockEventBus.Setup(eb => eb.PublishAsync(It.IsAny<IntegrationEvent>())).Returns(Task.CompletedTask);
        var request = new AddParcelCommand(invoiceId, fullName, mobileNo, address, codAmount, note);
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/orders", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299

        // Deserialize and verify response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var parcelDto = JsonConvert.DeserializeObject<ParcelDto>(responseContent);



        parcelDto.Should().NotBeNull();

        // Verify the IEventBus was called
        _factory.MockEventBus.Verify(eb => eb.PublishAsync(It.IsAny<IntegrationEvent>()), Times.AtLeast(1));

        // Add more assertions as needed
    }

    [Fact]
    public async Task AddAsync_Valid_AddParcelCommandHandler_Request()
    {
        // Arrange   
        var command = new AddParcelCommand("123456", "John Doe", "1234567890", "123 Main St", 50.00m, "Test note");
        var handler = new AddParcelCommandHandler(_parcelRepositoryMock.Object, _iVaildator.Object);

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
        var command = new AddParcelCommand("", "John Doe", "1234567890", "123 Main St", 50.00m, "Test note");
        var handler = new AddParcelCommandHandler(_parcelRepositoryMock.Object, _iVaildator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(command, default));
    }

}
