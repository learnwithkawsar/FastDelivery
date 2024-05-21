using System.Net;

namespace FastDelivery.Framework.Core.Exceptions;
public class NotFoundException : CustomException
{
    public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }
}
