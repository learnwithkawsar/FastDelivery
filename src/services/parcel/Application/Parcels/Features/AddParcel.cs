using FastDelivery.Service.Order.Application.Parcels.Dtos;
using FastDelivery.Service.Order.Domain;
using FluentValidation;
using MapsterMapper;
using MediatR;

namespace FastDelivery.Service.Order.Application.Parcels.Features;
public static class AddParcel
{
    public sealed record Command : IRequest<ParcelDto>
    {
        public readonly AddParcelDto addParcelDto;
        public Command(AddParcelDto entity)
        {
            addParcelDto = entity;
        }
    }

    public sealed class Vailator : AbstractValidator<Command>
    {
        public Vailator(IParcelRepository parcelRepository) 
        {
            RuleFor(p => p.addParcelDto.InvoiceId)
                .NotEmpty()
                .MaximumLength(75)
                .WithName("InvoiceId");
              RuleFor(p => p.addParcelDto.FullName)
                .NotEmpty()
                .MaximumLength(75)
                .WithName("FullName");
              RuleFor(p => p.addParcelDto.MobileNo)
                .NotEmpty()
                .MaximumLength(75)
                .WithName("MobileNo");
              RuleFor(p => p.addParcelDto.Address)
                .NotEmpty()
                .MaximumLength(75)
                .WithName("Address");
              RuleFor(p => p.addParcelDto.COD_Amount)
                .GreaterThan(0)
                .WithName("COD_Amount");

        }
    }
    public sealed class Handler: IRequestHandler<Command,ParcelDto>
    {
        private readonly IParcelRepository _parcelRepository;
        private readonly IMapper _mapper;
        public Handler(IParcelRepository parcelRepository, IMapper mapper)
        {
            _parcelRepository = parcelRepository;
            _mapper = mapper;
        }

        public async Task<ParcelDto> Handle(Command request, CancellationToken cancellationToken)
        {
            string parcelId = "0987654321";
            var parcelToAdd = ParcelInfo.Create(
                parcelId,
                request.addParcelDto.InvoiceId,
                request.addParcelDto.FullName,
                request.addParcelDto.MobileNo,
                request.addParcelDto.Address,
                request.addParcelDto.COD_Amount,
                request.addParcelDto.Note
                );
            await _parcelRepository.AddAsync(parcelToAdd,cancellationToken);
            return _mapper.Map<ParcelDto>(parcelToAdd);
        }
    }
}
