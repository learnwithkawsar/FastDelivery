using FastDelivery.Framework.Core.Pagination;
using FastDelivery.Framework.Infrastructure.Controllers;
using FastDelivery.Service.Order.Application.Parcels.Dtos;
using FastDelivery.Service.Order.Application.Parcels.Features;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
public class ParcelController : VersionedApiController
{

    [HttpPost(Name = nameof(AddParcelAsync))]
    //[Authorize("catalog:write")]
    [ProducesResponseType(201, Type = typeof(ParcelDto))]
    //[SwaggerOperation(Summary = "Deletes a specific TodoItem")]
    public async Task<ParcelDto> AddParcelAsync(AddParcelDto request)
    {
        var command = new AddParcel.Command(request);
        var commandResponse = await Mediator.Send(command);

        return commandResponse;
    }

    [HttpPost("search")]   
    [ProducesResponseType(200, Type = typeof(PagedList<ParcelDto>))]  
    public async Task<PagedList<ParcelDto>> SearchParcelAsync([FromQuery] ParcelParametersDto parameters)
    {
        var command = new SearchParcels.Query(parameters);
        var commandResponse = await Mediator.Send(command);

        return commandResponse;
    }
}
