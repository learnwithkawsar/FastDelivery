using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Framework.Infrastructure.Options;
public class AppOptions : IOptionsRoot
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = "FSH.WebAPI";
}
