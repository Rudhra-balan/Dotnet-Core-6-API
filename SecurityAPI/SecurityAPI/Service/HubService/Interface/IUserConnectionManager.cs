namespace SecurityAPI.Service.HubService.Interface;

public interface IUserConnectionManager
{
    void KeepUserConnection(string userId, string connectionId);
    void RemoveUserConnection(string connectionId);
    List<string> GetUserConnections(string userId);
}