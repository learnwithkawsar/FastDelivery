using FastDelivery.Service.Identity.Domain.Users;
using FastDelivery.Service.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FastDelivery.Service.Identity.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    public ValuesController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    // GET: api/<ValuesController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<ValuesController>/5
    [HttpGet("{id}")]
    public async Task<string> GetAsync(int id)
    {
     var   adminUser = new ApplicationUser
        {
            
            Email = "admin@admin.com",
            UserName = "admin",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            NormalizedEmail = "admin@admin.com".ToUpperInvariant(),
            NormalizedUserName = "admin".ToUpperInvariant(),
            
        };

       
        var password = new PasswordHasher<ApplicationUser>();
        adminUser.PasswordHash = password.HashPassword(adminUser, "123Pa$$word!");
        await _userManager.CreateAsync(adminUser);
        return "value";
    }

    // POST api/<ValuesController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
        
    }

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
