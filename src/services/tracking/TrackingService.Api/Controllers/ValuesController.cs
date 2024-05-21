using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace TrackingService.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly DaprClient _daprClient;

    public ValuesController(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    [HttpGet]
    public async Task<string> GetAsync()
    {
        string res = "";
        try
        {
            await _daprClient.SaveStateAsync("statestore", "orderId", Guid.NewGuid());

        }
        catch (Exception ex)
        {

            throw;
        }
        return res;
    }
}
