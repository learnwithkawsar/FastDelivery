using FastDelivery.Framework.Core.Pagination;

namespace FastDelivery.Service.Order.Application.Parcels.Dtos;
public class ParcelParametersDto : PaginationParameters
{
    public string? Keyword { get; set; }
}
