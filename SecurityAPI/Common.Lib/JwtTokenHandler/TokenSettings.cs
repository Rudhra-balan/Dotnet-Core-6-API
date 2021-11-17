namespace Common.Lib.JwtTokenHandler;

public class TokenSettings
{
    public string SecretKey { get; set; }

    public string PrivateKey { get; set; }
    public string PublicKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int NotBeforeMinutes { get; set; }
    public int ExpirationMinutes { get; set; }
}