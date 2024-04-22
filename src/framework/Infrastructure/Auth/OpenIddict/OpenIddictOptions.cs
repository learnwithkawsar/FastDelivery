using FastDelivery.Framework.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Framework.Infrastructure.Auth.OpenIddict;
public class OpenIddictOptions : IOptionsRoot
{
    [Required(AllowEmptyStrings = false)]
    public string? ClientId { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string? ClientSecret { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string? IssuerUrl { get; set; } = string.Empty;
}
