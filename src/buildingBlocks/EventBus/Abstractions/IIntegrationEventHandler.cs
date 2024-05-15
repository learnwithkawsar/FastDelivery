using FastDelivery.BuildingBlocks.EventBus.Events;
using FastDelivery.Framework.Core.Services;

namespace FastDelivery.BuildingBlocks.EventBus.Abstractions;
public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}

public interface IIntegrationEventHandler 
{
}