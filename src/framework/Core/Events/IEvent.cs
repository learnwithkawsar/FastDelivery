using MediatR;
namespace FastDelivery.Framework.Core.Events;
public interface IEvent : INotification
{
    DefaultIdType Id { get; }
    DateTime CreationDate { get; }
    IDictionary<string, object> MetaData { get; }
}