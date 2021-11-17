using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Common.Lib.JwtTokenHandler;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecurityAPI.Model;

namespace SecurityAPI.Service;

public class TokenService : ITokenService
{
    private readonly TokenSettings _settings;

    #region Constructor

    public TokenService(IOptionsSnapshot<TokenSettings> settings)
    {
        _settings = settings.Value;
    }

    #endregion

    #region Pubilc Member

    public AccessTokenModel GenerateAccessToken(UserModel user)
    {
        var secretKey = Convert.FromBase64String($"{_settings.PrivateKey}");

        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(secretKey, out _);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new CryptoProviderFactory {CacheSignatureProviders = false}
        };


        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now.AddMinutes(_settings.NotBeforeMinutes),
            Expires = DateTime.Now.AddMinutes(_settings.ExpirationMinutes),
            SigningCredentials = signingCredentials,
            Subject = new ClaimsIdentity(GetClaim(user))
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(descriptor);
        var encryptedJwt = tokenHandler.WriteToken(securityToken);
        return new AccessTokenModel
        {
            AccessToken = encryptedJwt,
            RefreshToken = GenerateRefreshToken(),
            ExpiresAt = (int) TimeSpan.FromMinutes(_settings.ExpirationMinutes).TotalMilliseconds
        };
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var publicKey = Convert.FromBase64String($"{_settings.PublicKey}");

        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(publicKey, out _);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _settings.Issuer,
            ValidAudience = _settings.Audience,
            IssuerSigningKey = new RsaSecurityKey(rsa),

            CryptoProviderFactory = new CryptoProviderFactory()
            {
                CacheSignatureProviders = false
            }
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

        if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.RsaSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    #endregion


    #region Private Member

    private string GenerateRefreshToken(int size = 32)
    {
        var randomNumber = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private List<Claim> GetClaim(UserModel userInfo)
    {
        var now = DateTime.Now;
        var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();
        return new List<Claim>
        {
            new(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.AuthTime, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new(ClaimType.NameId, userInfo.Id.ToString()),
            new(ClaimType.Surname, userInfo.LastName),
            new(ClaimType.GivenName, userInfo.FirstName),
            new(ClaimType.Email, userInfo.Email),
            new(ClaimType.Username, userInfo.Username)
        };
    }

    #endregion
}