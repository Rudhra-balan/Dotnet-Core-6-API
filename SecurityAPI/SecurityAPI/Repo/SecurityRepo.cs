using System.Globalization;
using Common.Lib.Enumeration;
using Common.Lib.Exceptions;
using Common.Lib.Extenstion;
using Common.Lib.ResponseHandler.Resources;
using Common.Lib.Security;
using SecurityAPI.Model;
using SecurityAPI.Repo.Interfaces;

namespace SecurityAPI.Repo;

public class SecurityRepo : ISecurityRepo
{
    public async Task<UserModel> AuthenticateUserAsync(string username, string password)
    {
        var users = GetUsers();

        var userEntity = users.FirstOrDefault(user =>
            user.Username == username);
        if (userEntity == null) throw new UnAuthorizedException(ResponseMessage.InvalidCredentials);

        if (userEntity.Password != Cryptographer.HashPassword(password))
            throw new UnAuthorizedException(ResponseMessage.InvalidPasswordException);

        return await Task.FromResult(userEntity);
    }

    public async Task<UserModel> GetUserInformationByUserIdAsync(Guid userId)
    {
        var userInfo = GetUsers().FirstOrDefault(user => user.Id == userId);
        if (userInfo == null) throw new UnAuthorizedException(ResponseMessage.UnAuthorized);

        return await Task.FromResult(userInfo);
    }

    #region [Static Data]

    private List<UserModel> GetUsers()
    {
        var users = new List<UserModel>();

        var sysAdmin = new UserModel
        {
            Username = "admin",
            FirstName = "system",
            LastName = "admin",
            Email = "sysAdmin@test.com",
            Id = "381dc60d-cbd0-4292-abbe-5ab2db2cee11".ToGuid(),
            Role = UserRole.Admin,
            Password = Cryptographer.HashPassword("admin"),
            LastLoginDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
        };

        users.Add(sysAdmin);

        var sysUser = new UserModel
        {
            Username = "user",
            FirstName = "system",
            LastName = "user",
            Email = "sysUser@test.com",
            Id = "b3e5f95a-d362-4e7a-89a7-5517909b148f".ToGuid(),
            Role = UserRole.User,
            Password = Cryptographer.HashPassword("user"),
            LastLoginDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
        };

        users.Add(sysUser);


        var sysReader = new UserModel
        {
            Username = "reader",
            FirstName = "system",
            LastName = "reader",
            Email = "sysUser@test.com",
            Id = "96ac6b9d-28f5-449e-9bd5-09e9d94ddbaa".ToGuid(),
            Role = UserRole.Reader,
            Password = Cryptographer.HashPassword("reader"),
            LastLoginDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
        };

        users.Add(sysReader);

        return users;
    }

    #endregion
}