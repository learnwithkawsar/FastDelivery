using FastDelivery.Framework.Core.Domain;

namespace FastDelivery.Service.Order.Domain;

public class ParcelInfo : BaseEntity
{
    public static ParcelInfo Create(string parcelId, string invoiceId, string fullName, string mobileNo, string address, decimal cOD_Amount, string? note)
    {
        ParcelInfo parcelInfo = new()
        {

            ParcelId = parcelId!,
            InvoiceId = invoiceId!,
            FullName = fullName!,
            MobileNo = mobileNo!,
            Address = address!,
            COD_Amount = cOD_Amount!,
            Note = note
        };
        //var @event = new ParcelCreateEvent()
        return parcelInfo;
    }

    public required string ParcelId { get; set; }
    public required string InvoiceId { get; set; }
    public required string FullName { get; set; }
    public required string MobileNo { get; set; }
    public required string Address { get; set; }
    public decimal COD_Amount { get; set; }
    public string? Note { get; set; } 

}
