namespace SignalR.ClientConsole;

public record LoginRequestModel
{
    public string Username { get; set; }

    public string Password { get; set; }
}

public record AccessTokenModel
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public int ExpiresAt { get; set; }
}