using FastDelivery.Framework.Core.Pagination;
using FastDelivery.Service.Order.Application.Parcels.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Service.Order.Application.Parcels.Features;
public class SearchParcels
{
    public sealed record Query : IRequest<PagedList<ParcelDto>>
    {
        public readonly ParcelParametersDto parameters;
       
        public Query(ParcelParametersDto parameters)
        {
            this.parameters = parameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<ParcelDto>>
    {
        private readonly IParcelRepository _repository;

        public Handler(IParcelRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<ParcelDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _repository.GetPagedParcelsAsync<ParcelDto>(request.parameters, cancellationToken);
        }
    }
}
