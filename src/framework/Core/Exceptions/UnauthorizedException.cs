using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Framework.Core.Exceptions;
public class UnauthorizedException : CustomException
{
    public string Error { get; set; }
    public string Description { get; set; }
    public UnauthorizedException(string error = default!, string description = default!) : base(error, HttpStatusCode.Unauthorized)
    {
        Error = error;
        Description = description;
    }
}
