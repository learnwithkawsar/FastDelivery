using Microsoft.AspNetCore.Mvc;

namespace FastDelivery.Framework.Infrastructure.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
public class VersionedApiController : BaseApiController
{
}