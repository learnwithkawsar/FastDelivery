using Dapr.Client;
using FastDelivery.Framework.Infrastructure.Multitenancy;
using FastDelivery.Service.Identity.Domain.Users;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FastDelivery.Service.Identity.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private string DAPR_STORE_NAME = "statestore";
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DaprClient _daprClient;
    private readonly IMultiTenantStore<AppTenantInfo> _tenantStore;
    public ValuesController(UserManager<ApplicationUser> userManager, DaprClient daprClient, IMultiTenantStore<AppTenantInfo> tenantStore)
    {
        _userManager = userManager;
        _daprClient = daprClient;
        _tenantStore = tenantStore;
    }
    // GET: api/<ValuesController>
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> GetAsync()
    {
        var forecasts = await _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(
                 HttpMethod.Get,
                 "parcelservice",
                 "weatherForecast");
        var httpClient = DaprClient.CreateInvokeHttpClient("parcelservice");
        var res = await httpClient.GetAsync("/weatherForecast");
        var data = await res.Content.ReadAsStringAsync();

        var date = await _daprClient.GetStateAsync<DateTime>(DAPR_STORE_NAME, "date");
        return forecasts;
    }

    // GET api/<ValuesController>/5

    [HttpGet("{id}")]
    public async Task<IEnumerable<AppTenantInfo>> GetAsync(int id)
    {



        var data = await _tenantStore.GetAllAsync();
        return data;
        //var adminUser = new ApplicationUser
        //{

        //    Email = "admin@admin.com",
        //    UserName = "admin",
        //    EmailConfirmed = true,
        //    PhoneNumberConfirmed = true,
        //    NormalizedEmail = "admin@admin.com".ToUpperInvariant(),
        //    NormalizedUserName = "admin".ToUpperInvariant(),

        //};


        //var password = new PasswordHasher<ApplicationUser>();
        //adminUser.PasswordHash = password.HashPassword(adminUser, "123Pa$$word!");
        //await _userManager.CreateAsync(adminUser);
        //return "value";
    }

    //[HttpPost]
    //[Topic("fastdelivery-pubsub", "testtopic1")]
    //// POST api/<ValuesController>

    //public async Task<string> Post([FromBody] string value)
    //{
    //    return value;
    //}

    // PUT api/<ValuesController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ValuesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
