using FastDelivery.Framework.Core.Exceptions;
using System.Net;

namespace FastDelivery.Service.Identity.Application.Users;
public class UserRegistrationException : CustomException
{
    public UserRegistrationException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, statusCode)
    {
    }
}