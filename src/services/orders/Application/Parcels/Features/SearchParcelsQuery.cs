namespace FastDelivery.Service.Order.Application.Parcels.Features;
public record SearchParcelsQuery(ParcelParametersDto parameters) : IRequest<PagedList<ParcelDto>>
{
}

public class SearchParcelsQueryHandler : IRequestHandler<SearchParcelsQuery, PagedList<ParcelDto>>
{
    private readonly IParcelRepository _repository;

    public SearchParcelsQueryHandler(IParcelRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedList<ParcelDto>> Handle(SearchParcelsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetPagedParcelsAsync<ParcelDto>(request.parameters, cancellationToken);
    }
}

