using FastDelivery.BuildingBlocks.EventBus.Abstractions;
using FastDelivery.Framework.Core.Pagination;
using FastDelivery.Framework.Infrastructure.Controllers;
using FastDelivery.Service.Order.Application.IntegrationEvents.Events;
using FastDelivery.Service.Order.Application.Parcels.Dtos;
using FastDelivery.Service.Order.Application.Parcels.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Api.Controllers;
public class OrdersController : VersionedApiController
{
    private readonly IEventBus _eventBus;
    private readonly ISender _immedator;

    public OrdersController(IEventBus eventBus, ISender immedator)
    {
        _eventBus = eventBus;
        _immedator = immedator;
    }

   // [AllowAnonymous]
    [HttpPost(Name = nameof(AddAsync))]
    [ProducesResponseType(201, Type = typeof(ParcelDto))]    
    public async Task<ParcelDto> AddAsync(AddParcelDto request)
    {
       
        try
        {
            var command = new AddParcelCommand(request.InvoiceId,request.FullName,request.MobileNo,request.Address,request.COD_Amount,request.Note);
            var commandResponse = await _immedator.Send(command);
            await _eventBus.PublishAsync(new OrderAddToTrackingIntegrationEvent(commandResponse));
            return commandResponse;
        }
        catch (Exception ex)
        {
            throw;
        }

       
    }

    [HttpPost("search")]   
    [ProducesResponseType(200, Type = typeof(PagedList<ParcelDto>))]  
    public async Task<PagedList<ParcelDto>> SearchAsync([FromQuery] ParcelParametersDto parameters)
    {
        var command = new SearchParcelsQuery(parameters);
        var commandResponse = await Mediator.Send(command);    
        return commandResponse;
    }
}
