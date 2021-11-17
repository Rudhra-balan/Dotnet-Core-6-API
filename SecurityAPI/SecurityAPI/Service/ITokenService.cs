using System.Security.Claims;
using SecurityAPI.Model;

namespace SecurityAPI.Service;

public interface ITokenService
{
    AccessTokenModel GenerateAccessToken(UserModel user);

    ClaimsPrincipal GetPrincipalFromToken(string token);
}