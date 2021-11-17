using System.ComponentModel.DataAnnotations;

namespace SecurityAPI.Model;

public record AccessTokenModel
{
    [Required] public string AccessToken { get; set; }

    [Required] public string RefreshToken { get; set; }

    public int ExpiresAt { get; set; }
}