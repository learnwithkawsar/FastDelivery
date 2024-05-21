namespace FastDelivery.Service.Order.Application.Parcels.Dtos;
public class AddParcelDto
{
    public required string InvoiceId { get; set; }
    public required string FullName { get; set; }
    public required string MobileNo { get; set; }
    public required string Address { get; set; }
    public decimal COD_Amount { get; set; }
    public string? Note { get; set; }
    public int MerchantId { get; set; }
}
