using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace AutoriaStore.Infrastructure.Services;

public sealed class AuthenticatedUserService(IHttpContextAccessor httpContextAccessor) : IAuthenticatedUserService
{
    public Guid? GetUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;

        var subjectValue = user?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(subjectValue, out var userId) ? userId : null;
    }
}
