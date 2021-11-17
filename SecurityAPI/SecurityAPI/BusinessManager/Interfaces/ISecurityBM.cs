using SecurityAPI.Model;

namespace SecurityAPI.BusinessManager.Interfaces;

public interface ISecurityBM
{
    Task<AccessTokenModel> AuthenticateAsync(LoginRequestModel loginRequestModel);

    Task<AccessTokenModel> RefreshTokenAsync(AccessTokenModel refreshTokenRequestModel);
}