namespace FastDelivery.Service.Order.Application.Parcels.Features;
public record GetParcelDetailsQuery(string invoiceId) : IRequest<ParcelDto>
{
}

public class GetParcelDetailsQueryHandler : IRequestHandler<GetParcelDetailsQuery, ParcelDto>
{
    private readonly IParcelRepository _repository;

    public GetParcelDetailsQueryHandler(IParcelRepository repository)
    {
        _repository = repository;
    }

    public async Task<ParcelDto> Handle(GetParcelDetailsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetParcelAsync<ParcelDto>(request.invoiceId, cancellationToken);
    }
}

