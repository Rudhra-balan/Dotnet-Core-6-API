using Common.Lib.Caching;
using Common.Lib.ResponseHandler.Resources;
using SecurityAPI.BusinessManager.Interfaces;
using SecurityAPI.Model;
using SecurityAPI.Repo.Interfaces;
using SecurityAPI.Service;
using Common.Lib.Exceptions;
using Common.Lib.Extenstion;
using Common.Lib.JwtTokenHandler;

namespace SecurityAPI.BusinessManager;

public class SecurityBM : ISecurityBM
{
    #region constructor

    public SecurityBM(ITokenService iTokenService, ISecurityRepo securityRepo, ICacheService cacheService)
    {
        _tokenService = iTokenService;
        _securityRepo = securityRepo;
        _cacheService = cacheService;
    }

    #endregion

    #region Private variable

    private readonly ITokenService _tokenService;
    private readonly ISecurityRepo _securityRepo;
    private readonly ICacheService _cacheService;

    #endregion

    #region Public Method

    public async Task<AccessTokenModel> AuthenticateAsync(LoginRequestModel loginRequestModel)
    {
        AccessTokenModel accessTokenDO;
        try
        {
            var userInfo =
                await _securityRepo.AuthenticateUserAsync(loginRequestModel.Username, loginRequestModel.Password);

            if (userInfo == null)
                throw new UnAuthorizedException(ResponseMessage.UnAuthorized);

            accessTokenDO = PopulateToken(userInfo);

            _cacheService.GetOrAdd($"{CacheKeys.AccessToken}-{userInfo.Id}", () => accessTokenDO);
        }
        catch (Exception ex)
        {
            throw new BaseException(ex);
        }

        return accessTokenDO;
    }

    public async Task<AccessTokenModel> RefreshTokenAsync(AccessTokenModel refreshTokenRequestModel)
    {
        AccessTokenModel tokenModel;
        try
        {
            var claimsPrincipal = _tokenService.GetPrincipalFromToken(refreshTokenRequestModel.AccessToken);
            var userCliam = claimsPrincipal.Claims.First(c => c.Type == ClaimType.NameId);
            if (userCliam == null) throw new BadRequestException(ResponseMessage.InvalidRefreshToken);

            var userInfo = await _securityRepo.GetUserInformationByUserIdAsync(userCliam.Value.ToGuid());

            if (userInfo == null) throw new BadRequestException(ResponseMessage.InvalidRefreshToken);
            var jwtToken = _tokenService.GenerateAccessToken(userInfo);
            if (jwtToken.IsAnyNullOrEmpty()) throw new BadRequestException(ResponseMessage.TokenGeneratedException);

            _cacheService.GetOrAdd($"{CacheKeys.AccessToken}-{userInfo.Id}", () => jwtToken);
            tokenModel = jwtToken;
        }
        catch (Exception ex)
        {
            throw new BaseException(ex);
        }

        return tokenModel;
    }

    #endregion

    private AccessTokenModel PopulateToken(UserModel userModel)
    {
        var accessToken = _tokenService.GenerateAccessToken(userModel);
        if (accessToken.IsAnyNullOrEmpty())
            throw new InternalServerErrorException(ResponseMessage.TokenGeneratedException);

        return accessToken;
    }
}