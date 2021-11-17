using System.Security.Claims;
using Common.Lib.JwtTokenHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using SecurityAPI.Service.HubService.Interface;

namespace SecurityAPI.Service.HubService;

[Authorize]
public class SignalRContext : Hub<IHubClient>
{
    private readonly IUserConnectionManager _userConnectionManager;

    public SignalRContext(IUserConnectionManager userConnectionManager)
    {
        _userConnectionManager = userConnectionManager;
    }

    public override Task OnConnectedAsync()
    {
        var identity = Context.User;
        if (identity != null)
        {
            var userId = GetUserId(identity).ToString();
            _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        var connectionId = Context.ConnectionId;
        _userConnectionManager.RemoveUserConnection(connectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public Guid GetUserId(ClaimsPrincipal principal)
    {
        return Guid.TryParse(principal.FindFirstValue(ClaimType.NameId), out var userId) ? userId : default;
    }
}