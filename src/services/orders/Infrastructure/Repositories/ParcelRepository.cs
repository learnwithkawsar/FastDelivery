using FastDelivery.Framework.Core.Pagination;
using FastDelivery.Framework.Core.Services;
using FastDelivery.Framework.Persistence.Mongo;
using FastDelivery.Service.Order.Application.Parcels;
using FastDelivery.Service.Order.Application.Parcels.Dtos;
using FastDelivery.Service.Order.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FastDelivery.Service.Order.Infrastructure.Repositories;
public class ParcelRepository : MongoRepository<ParcelInfo, Guid>, IParcelRepository
{
    private readonly IMongoDbContext _dbContext;
    public ParcelRepository(IMongoDbContext context, IDateTimeService dateTimeService) : base(context, dateTimeService)
    {
        _dbContext = context;
    }

    public async Task<PagedList<ParcelDto>> GetPagedParcelsAsync<ParcelDto>(ParcelParametersDto parameters, CancellationToken cancellationToken = default)
    {
        var queryable = _dbContext.GetCollection<ParcelInfo>().AsQueryable();
        if (!string.IsNullOrEmpty(parameters.Keyword))
        {
            string keyword = parameters.Keyword.ToLower();
            queryable = queryable.Where(t => t.FullName.ToLower().Contains(keyword)
            || t.MobileNo.ToLower().Contains(keyword)
            || t.InvoiceId.ToLower().Contains(keyword));
        }
        queryable = queryable.OrderBy(p => p.CreatedOn);
        return await queryable.ApplyPagingAsync<ParcelInfo, ParcelDto>(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }
}
