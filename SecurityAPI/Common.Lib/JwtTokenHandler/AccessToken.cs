namespace Common.Lib.JwtTokenHandler;

public class AccessTokenDO
{
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
}