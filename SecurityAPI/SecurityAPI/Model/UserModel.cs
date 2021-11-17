using Common.Lib.Enumeration;

namespace SecurityAPI.Model;

public class UserModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string LastName { get; set; }
    public UserRole Role { get; set; }
    public string LastLoginDate { get; set; }
}