using FastDelivery.BuildingBlocks.EventBus.Events;
using FastDelivery.Service.Order.Application.Parcels.Dtos;

namespace FastDelivery.Service.Order.Application.IntegrationEvents.Events;
public record OrderAddToTrackingIntegrationEvent(ParcelDto ParcelDto) : IntegrationEvent;