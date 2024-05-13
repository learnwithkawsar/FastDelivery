using FastDelivery.Framework.Core.Database;
using FastDelivery.Framework.Core.Pagination;
using FastDelivery.Service.Order.Application.Parcels.Dtos;
using FastDelivery.Service.Order.Domain;

namespace FastDelivery.Service.Order.Application.Parcels;
public interface IParcelRepository : IRepository<ParcelInfo, Guid>
{
    Task<PagedList<ParcelDto>> GetPagedParcelsAsync<ParcelDto>(ParcelParametersDto parameters, CancellationToken cancellationToken = default);

}
