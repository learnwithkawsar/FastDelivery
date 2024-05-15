using FastDelivery.BuildingBlocks.EventBus.Abstractions;
using FastDelivery.Framework.Core.Pagination;
using FastDelivery.Framework.Infrastructure.Controllers;
using FastDelivery.Service.Order.Application.IntegrationEvents.Events;
using FastDelivery.Service.Order.Application.Parcels.Dtos;
using FastDelivery.Service.Order.Application.Parcels.Features;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Api.Controllers;
public class OrdersController : VersionedApiController
{
    private readonly IEventBus _eventBus;

    public OrdersController(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    [HttpPost(Name = nameof(AddAsync))]
    [ProducesResponseType(201, Type = typeof(ParcelDto))]    
    public async Task<ParcelDto> AddAsync(AddParcelDto request)
    {
        var command = new AddParcel.Command(request);
        var commandResponse = await Mediator.Send(command);
        try
        {
            await _eventBus.PublishAsync(new OrderAddToTrackingIntegrationEvent(commandResponse));          
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
