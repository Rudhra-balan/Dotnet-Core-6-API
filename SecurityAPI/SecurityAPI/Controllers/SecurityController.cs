using Common.Lib.Exceptions.ErrorHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityAPI.BusinessManager.Interfaces;
using SecurityAPI.Model;

namespace SecurityAPI.Controllers;

public class SecurityController : ControllerBase
{
    #region Private Variable

    private readonly ISecurityBM _securityBM;

    #endregion

    #region Constructor

    public SecurityController(ISecurityBM securityBM)
    {
        _securityBM = securityBM;
    }

    #endregion

    #region Public Member

    [HttpPost("Authenticate")]
    [ValidateModel]
    [AllowAnonymous]
    public async Task<AccessTokenModel> Authenticate([FromBody] LoginRequestModel loginRequestDO)
    {
        return await _securityBM.AuthenticateAsync(loginRequestDO).ConfigureAwait(false);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<AccessTokenModel> RefreshToken([FromBody] AccessTokenModel exchangeRefreshTokenRequestDo)
    {
        return await _securityBM.RefreshTokenAsync(exchangeRefreshTokenRequestDo);
    }

    #endregion
}