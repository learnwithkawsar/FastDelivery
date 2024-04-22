using FastDelivery.Framework.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Framework.Infrastructure.Auth.OpenId;
public class OpenIdOptions : IOptionsRoot
{
    [Required(AllowEmptyStrings = false)]
    public string? Authority { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string? Audience { get; set; } = string.Empty;
}
