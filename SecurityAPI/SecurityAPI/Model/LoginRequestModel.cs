using System.ComponentModel.DataAnnotations;

namespace SecurityAPI.Model;

public record LoginRequestModel
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}