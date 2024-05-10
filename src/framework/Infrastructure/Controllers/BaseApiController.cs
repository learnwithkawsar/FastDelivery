using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FastDelivery.Framework.Infrastructure.Controllers;
[ApiController]
[Authorize]
[Route("[controller]")]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected ILogger<BaseApiController> _Logger => HttpContext.RequestServices.GetRequiredService<ILogger<BaseApiController>>();

}
