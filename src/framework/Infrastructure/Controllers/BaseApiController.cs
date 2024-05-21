using FastDelivery.BuildingBlocks.EventBus.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DaprClient = Dapr.Client.DaprClient;

namespace FastDelivery.Framework.Infrastructure.Controllers;
[ApiController]
[Authorize]
[Route("[controller]")]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;
    private IEventBus _eventBus = null!;
    private DaprClient _daprClient = null;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected IEventBus EventBus => _eventBus ??= HttpContext.RequestServices.GetRequiredService<IEventBus>();
    protected DaprClient DaprClientInstance => _daprClient ??= HttpContext.RequestServices.GetRequiredService<DaprClient>();

    protected ILogger<BaseApiController> _Logger => HttpContext.RequestServices.GetRequiredService<ILogger<BaseApiController>>();

}
