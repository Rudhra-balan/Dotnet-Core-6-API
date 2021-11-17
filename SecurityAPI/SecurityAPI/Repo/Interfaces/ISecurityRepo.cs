using SecurityAPI.Model;

namespace SecurityAPI.Repo.Interfaces;

public interface ISecurityRepo
{
    Task<UserModel> AuthenticateUserAsync(string username, string password);

    Task<UserModel> GetUserInformationByUserIdAsync(Guid userId);
}