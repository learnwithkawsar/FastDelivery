using FastDelivery.Framework.Core.Pagination;
using FastDelivery.Framework.Infrastructure.Controllers;
using FastDelivery.Service.Order.Application.Parcels.Dtos;
using FastDelivery.Service.Order.Application.Parcels.Features;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Api.Controllers;
public class OrdersController : VersionedApiController
{

    [HttpPost(Name = nameof(AddAsync))]
    //[Authorize("catalog:write")]
    [ProducesResponseType(201, Type = typeof(ParcelDto))]
    //[SwaggerOperation(Summary = "Deletes a specific TodoItem")]
    public async Task<ParcelDto> AddAsync(AddParcelDto request)
    {
        var command = new AddParcel.Command(request);
        var commandResponse = await Mediator.Send(command);
        try
        {
            await DaprClientInstance.PublishEventAsync("fastdelivery-pubsub", "testtopic1", commandResponse);
        }
        catch (Exception ex)
        {

            throw;
        }

        return commandResponse;
    }

    [HttpPost("search")]   
    [ProducesResponseType(200, Type = typeof(PagedList<ParcelDto>))]  
    public async Task<PagedList<ParcelDto>> SearchAsync([FromQuery] ParcelParametersDto parameters)
    {
        var command = new SearchParcels.Query(parameters);
        var commandResponse = await Mediator.Send(command);

        return commandResponse;
    }
}
