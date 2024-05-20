using FastDelivery.Service.Order.Application.Parcels.Dtos;
using FastDelivery.Service.Order.Domain;
using FluentValidation;
using Mapster;
using MediatR;
using System.ComponentModel.DataAnnotations;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Threading;

namespace FastDelivery.Service.Order.Application.Parcels.Features;
public record AddParcelCommand(string InvoiceId, string FullName, string MobileNo, string Address, decimal COD_Amount, string? Note)
    : IRequest<ParcelDto>
{
}


public  class Vailator : AbstractValidator<AddParcelCommand>
{
    public Vailator()
    {
        RuleFor(p => p.InvoiceId)
            .NotEmpty()
            .MaximumLength(75)
            .WithName("InvoiceId");
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
    private readonly IValidator<ParcelInfo> _validator;

    public AddParcelCommandHandler(IParcelRepository parcelRepository, IValidator<ParcelInfo> validator)
    {
        _parcelRepository = parcelRepository;
        _validator = validator;
    }

    public async Task<ParcelDto> Handle(AddParcelCommand request, CancellationToken cancellationToken)
    {
        string parcelId = "0987654321";
        var parcelToAdd = ParcelInfo.Create(
            parcelId,
            request.InvoiceId,
            request.FullName,
            request.MobileNo,
            request.Address,
            request.COD_Amount,
            request.Note
            );

        // Perform validation
        await ValidateParcelInfo(request, cancellationToken);

        await _parcelRepository.AddAsync(parcelToAdd, cancellationToken);
        return parcelToAdd.Adapt<ParcelDto>();
    }

    private async Task ValidateParcelInfo(AddParcelCommand addParcelCommand,CancellationToken cancellationToken)
    {
        //var validationContext = new ValidationContext(parcel);
        //var validationResults = new List<ValidationResult>();

        //bool isValid = Validator.TryValidateObject(parcel, validationContext, validationResults, true);

        //if (!isValid)
        //{
        //    var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
        //    throw new ArgumentException(errors);
        //}


        var validator = new Vailator();
        var validationResult = await validator.ValidateAsync(addParcelCommand, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ArgumentException(errors);
        }
    }
}

