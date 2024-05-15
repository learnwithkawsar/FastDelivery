

using FastDelivery.Framework.Core.Services;

namespace FastDelivery.BuildingBlocks.EventBus.Abstractions;

public interface IEventBus : IScopedService
{
    Task PublishAsync(IntegrationEvent integrationEvent);
}
