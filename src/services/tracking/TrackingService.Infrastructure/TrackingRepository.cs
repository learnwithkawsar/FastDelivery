using FastDelivery.Framework.Core.Services;
using FastDelivery.Framework.Persistence.Mongo;
using FastDelivery.Service.Tracking.Application.Trackings;
using FastDelivery.Service.Tracking.Application.Trackings.Dtos;
using FastDelivery.Service.Tracking.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Service.Tracking.Infrastructure;
public class TrackingRepository : MongoRepository<ParcelTrackingHistory, Guid>, ITrackingRepository
{
    private readonly IMongoDbContext _dbContext;
    public TrackingRepository(IMongoDbContext context, IDateTimeService dateTimeService) : base(context, dateTimeService)
    {
        _dbContext = context;
    }
   
}
