using FastDelivery.Framework.Core.Database;

namespace FastDelivery.Service.Order.Application.Parcels;
public interface IParcelRepository : IRepository<ParcelInfo, Guid>
{
    Task<PagedList<ParcelDto>> GetPagedParcelsAsync<ParcelDto>(ParcelParametersDto parameters, CancellationToken cancellationToken = default);
    Task<ParcelDto> GetParcelAsync<ParcelDto>(string invoiceId, CancellationToken cancellationToken = default);

}
