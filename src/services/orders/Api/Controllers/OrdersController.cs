using FastDelivery.BuildingBlocks.EventBus.Abstractions;
using FastDelivery.Framework.Core.Pagination;
using FastDelivery.Framework.Infrastructure.Controllers;
using FastDelivery.Service.Order.Application.IntegrationEvents.Events;
using FastDelivery.Service.Order.Application.Parcels.Dtos;
using FastDelivery.Service.Order.Application.Parcels.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Api.Controllers;
public class OrdersController : VersionedApiController
{

    public OrdersController()
    {

    }

    // [AllowAnonymous]
    [HttpPost(Name = nameof(AddAsync))]
    [ProducesResponseType(201, Type = typeof(ParcelDto))]
    public async Task<ParcelDto> AddAsync(AddParcelDto request)
    {

        try
        {
            var command = new AddParcelCommand(request.MerchantId, request.InvoiceId, request.FullName, request.MobileNo, request.Address, request.COD_Amount, request.Note);
            var commandResponse = await Mediator.Send(command);
            await EventBus.PublishAsync(new OrderAddToTrackingIntegrationEvent(commandResponse));
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
