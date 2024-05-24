using FastDelivery.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace FastDelivery.Service.Order.Domain;

public class ParcelInfo : BaseEntity
{
    public static ParcelInfo Create(int merchantId, string parcelId, string invoiceId, string fullName, string mobileNo, string address, decimal cOD_Amount, string? note, string currentStatus, string nextStatus)
    {
        ParcelInfo parcelInfo = new()
        {
            MerchantId = merchantId,
            ParcelId = parcelId!,
            InvoiceId = invoiceId!,
            FullName = fullName!,
            MobileNo = mobileNo!,
            Address = address!,
            COD_Amount = cOD_Amount!,
            Note = note,
            CurrentStatus = currentStatus,
            NextStatus = nextStatus
        };
        //var @event = new ParcelCreateEvent()
        return parcelInfo;
    }

    public int MerchantId { get; set; }
    public required string ParcelId { get; set; }
    [Required]
    [MaxLength(75)]
    public required string InvoiceId { get; set; }
    public required string FullName { get; set; }
    public required string MobileNo { get; set; }
    public required string Address { get; set; }
    public decimal COD_Amount { get; set; }
    public string? Note { get; set; }
    public string CurrentStatus { get; set; } = default!;
    public string NextStatus { get; set; } = string.Empty;

}
