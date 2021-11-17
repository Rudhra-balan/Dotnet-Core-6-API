using SecurityAPI.Service.HubService.Interface;

namespace SecurityAPI.Service.HubService;

public class UserConnectionManager : IUserConnectionManager
{
    private static readonly Dictionary<string, List<string>> UserRequestMap = new();
    private static readonly string UserRequestMapLocker = string.Empty;
    private static readonly Dictionary<string, List<string>> UserConnectionMap = new();
    private static readonly string UserConnectionMapLocker = string.Empty;

    public void KeepUserRequest(string userId, string requestId)
    {
        lock (UserRequestMapLocker)
        {
            if (!UserRequestMap.ContainsKey(userId)) UserRequestMap[userId] = new List<string>();
            UserRequestMap[userId].Add(requestId);
        }
    }

    public void KeepUserConnection(string userId, string connectionId)
    {
        lock (UserConnectionMapLocker)
        {
            if (!UserConnectionMap.ContainsKey(userId)) UserConnectionMap[userId] = new List<string>();
            UserConnectionMap[userId].Add(connectionId);
        }
    }

    public void RemoveUserConnection(string connectionId)
    {
        //Remove the connectionId of user 
        lock (UserConnectionMapLocker)
        {
            foreach (var userId in UserConnectionMap.Keys.Where(userId =>
                         UserConnectionMap.ContainsKey(userId) && UserConnectionMap[userId].Contains(connectionId)))
            {
                UserConnectionMap[userId].Remove(connectionId);
                break;
            }
        }
    }

    public List<string> GetUserConnections(string userId)
    {
        List<string> conn;
        lock (UserConnectionMapLocker)
        {
            conn = UserConnectionMap[userId];
        }

        return conn;
    }


    public string GetUserId(string requestId)
    {
        string userId;
        lock (UserRequestMapLocker)
        {
            userId = UserRequestMap
                .FirstOrDefault(keyValuePair => keyValuePair.Value.Contains(requestId))
                .Key;
        }

        return userId;
    }

    public void RemoveUserRequest(string requestId)
    {
        //Remove the connectionId of user 
        lock (UserRequestMapLocker)
        {
            foreach (var userId in UserRequestMap.Keys.Where(userId =>
                         UserRequestMap.ContainsKey(userId) && UserRequestMap[userId].Contains(requestId)))
            {
                UserRequestMap[userId].Remove(requestId);
                break;
            }
        }
    }
}