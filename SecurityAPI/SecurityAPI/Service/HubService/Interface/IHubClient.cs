namespace SecurityAPI.Service.HubService.Interface;

public interface IHubClient
{
    Task SendMessage(object message);
}