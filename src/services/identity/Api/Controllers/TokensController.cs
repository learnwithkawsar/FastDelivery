using FastDelivery.Service.Identity.Domain.Users;
using MassTransit.Internals;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Api.Controllers
{

    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokensController(IOpenIddictApplicationManager applicationManager, IOpenIddictScopeManager scopeManager, UserManager<ApplicationUser> userManager)
        {
            _applicationManager = applicationManager;
            _scopeManager = scopeManager;
            _userManager = userManager;
        }

        [HttpPost("~/connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ?? throw new ArgumentNullException();
            if (request.IsClientCredentialsGrantType())
            {
                return await HandleClientCredentialsGrantType(request);
            }
            else if (request.IsPasswordGrantType())
            {
                return await HandleResourceOwnerPasswordGrantType(request);
            }
            throw new NotImplementedException("The specified grant type is not implemented.");
        }

        private async Task<IActionResult> HandleClientCredentialsGrantType(OpenIddictRequest? request)
        {
            object? application = await _applicationManager.FindByClientIdAsync(request!.ClientId!) ?? throw new InvalidOperationException("The application details cannot be found in the database.");
            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);
            identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
            identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));
            identity.SetScopes(request!.GetScopes());
            identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
            identity.SetDestinations(GetDestinations);
            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        private async Task<IActionResult> HandleResourceOwnerPasswordGrantType(OpenIddictRequest request)
        {
            // Validate the username and password
            var username = request.Username;
            var password = request.Password;

            // You would typically validate the credentials against your user store/database
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest(new OpenIddictResponse
                {
                    Error = OpenIddictConstants.Errors.InvalidGrant,
                    ErrorDescription = "The username or password is invalid."
                });
            }

            // Create claims for the authenticated user
            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);
            identity.SetClaim(Claims.Subject, await _userManager.GetUserIdAsync(user));
            identity.SetClaim(Claims.Name, await _userManager.GetUserNameAsync(user));

            // Set scopes and resources as needed
            identity.SetScopes(request.GetScopes());
            identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
            identity.SetDestinations(GetDestinations);

            // Sign in the user and return the access token
            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private static IEnumerable<string> GetDestinations(Claim claim)
        {
            return claim.Type switch
            {
                Claims.Name or
                Claims.Subject
                    => new[] { Destinations.AccessToken, Destinations.IdentityToken },

                _ => new[] { Destinations.AccessToken },
            };
        }
    }
}
