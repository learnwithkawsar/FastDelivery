using Dapr;
using FastDelivery.Framework.Infrastructure.Controllers;
using FastDelivery.Service.Order.Application.Parcels.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrackingService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrackingOrdersController : ControllerBase
{
    // [Topic("orderpubsub", "testtopic")]
    [AllowAnonymous]
    [HttpPost(Name = "testtopic1")]
    [Topic("fastdelivery-pubsub", "testtopic1")]
    public async Task<ParcelDto> PostTrackingAsync([FromBody] ParcelDto request)
    {
        return request;
    }
}
