using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Service.Tracking.Application;
public class ParcelDto
{
    public Guid Id { get; set; }
    public string? ParcelId { get; set; }
    public string? InvoiceId { get; set; }
    public string? FullName { get; set; }
    public string? MobileNo { get; set; }
    public string? Address { get; set; }
    public decimal COD_Amount { get; set; }
    public string? Note { get; set; }
}
