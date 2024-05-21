using FastDelivery.Framework.Core.Database;

namespace FastDelivery.Service.Order.Application.Parcels;
public interface IParcelRepository : IRepository<ParcelInfo, Guid>
{
    Task<PagedList<ParcelDto>> GetPagedParcelsAsync<ParcelDto>(ParcelParametersDto parameters, CancellationToken cancellationToken = default);

}
