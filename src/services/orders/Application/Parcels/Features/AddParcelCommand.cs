namespace FastDelivery.Service.Order.Application.Parcels.Features;
public record AddParcelCommand(int MerchantId, string InvoiceId, string FullName, string MobileNo, string Address, decimal COD_Amount, string? Note)
    : IRequest<ParcelDto>
{
}

public class Vailator : AbstractValidator<AddParcelCommand>
{
    public Vailator(IParcelRepository parcelRepository)
    {
        RuleFor(p => p.InvoiceId)
            .NotEmpty()
            .MaximumLength(75)
            .WithName("InvoiceId");

        RuleFor(p => new { p.InvoiceId, p.MerchantId })
             .MustAsync(async (x, cancellation) =>
             {
                 var parcel = await parcelRepository.FindOneAsync(p => p.InvoiceId == x.InvoiceId && p.MerchantId == x.MerchantId, cancellation);
                 return parcel == null;
             })
             .WithMessage("The combination of InvoiceId and MarchatId must be unique.");

        RuleFor(p => p.FullName)
          .NotEmpty()
          .MaximumLength(75)
          .WithName("FullName");
        RuleFor(p => p.MobileNo)
          .NotEmpty()
          .MaximumLength(75)
          .WithName("MobileNo");
        RuleFor(p => p.Address)
          .NotEmpty()
          .MaximumLength(75)
          .WithName("Address");
        RuleFor(p => p.COD_Amount)
          .GreaterThan(0)
          .WithName("COD_Amount");

    }
}

public class AddParcelCommandHandler : IRequestHandler<AddParcelCommand, ParcelDto>
{
    private readonly IParcelRepository _parcelRepository;

    public AddParcelCommandHandler(IParcelRepository parcelRepository)
    {
        _parcelRepository = parcelRepository;
    }

    public async Task<ParcelDto> Handle(AddParcelCommand request, CancellationToken cancellationToken)
    {
        var rand = new Random();
        string parcelId = rand.Next(1000000, 99999999).ToString();
        var parcelToAdd = ParcelInfo.Create(
            request.MerchantId,
            parcelId,
            request.InvoiceId,
            request.FullName,
            request.MobileNo,
            request.Address,
            request.COD_Amount,
            request.Note,
            ParcelConstants.ORDER_PLACED,
            ParcelConstants.ORDER_PROCESSING
            );

        // Perform validation
        await ValidateParcelInfo(request, _parcelRepository, cancellationToken);

        await _parcelRepository.AddAsync(parcelToAdd, cancellationToken);
        return parcelToAdd.Adapt<ParcelDto>();
    }

    private async Task ValidateParcelInfo(AddParcelCommand addParcelCommand, IParcelRepository parcelRepository, CancellationToken cancellationToken)
    {
        var validator = new Vailator(parcelRepository);
        var validationResult = await validator.ValidateAsync(addParcelCommand, cancellationToken);
        if (!validationResult.IsValid)
        {
            string errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ArgumentException(errors);
        }
    }
}

