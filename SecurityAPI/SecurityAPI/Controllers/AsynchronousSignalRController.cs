using System.Security.Claims;
using Common.Lib.Extenstion;
using Common.Lib.JwtTokenHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SecurityAPI.BusinessManager.Interfaces;
using SecurityAPI.Model;
using SecurityAPI.Service.HubService;
using SecurityAPI.Service.HubService.Interface;

namespace SecurityAPI.Controllers;

public class AsynchronousSignalRController : Controller
{
    #region Private variable

    private readonly IHubContext<SignalRContext, IHubClient> _notification;

    private readonly IUserConnectionManager _userConnectionManager;

    private readonly ClaimsPrincipal _user;

    private readonly IAsynchronousSignalRBM _asynchronousSignalRbm;

    #endregion


    #region Constutor

    public AsynchronousSignalRController(IHubContext<SignalRContext, IHubClient> notification,
        IUserConnectionManager userConnectionManager, IHttpContextAccessor httpContextAccessor,
        IAsynchronousSignalRBM asynchronousSignalRbm)
    {
        _userConnectionManager = userConnectionManager;
        _notification = notification;
        _user = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
        _asynchronousSignalRbm = asynchronousSignalRbm;
    }

    #endregion


    #region Public Member

    [HttpGet("GetSystemInformation")]
    public string GetSystemInformation()
    {
        _asynchronousSignalRbm.OnCompleted -= AsynchronousSignalRbmOnOnCompleted;
        _asynchronousSignalRbm.OnCompleted += AsynchronousSignalRbmOnOnCompleted;
        _asynchronousSignalRbm.Execute(Guid.NewGuid());

        return "System Information will send through SignalR";
    }

    private void AsynchronousSignalRbmOnOnCompleted(object sender, CompletedEventArgs args)
    {
        if (args.Error != null) SendMessageToSignalRClient(new {Exception = args.Error.GetBaseException().Message});

        if (args.UserState != null) SendMessageToSignalRClient(args.UserState);
    }

    #endregion


    #region Private Method

    private void SendMessageToSignalRClient(object message)
    {
        var userId = GetUserId(_user).ToString();
        if (userId.IsNullOrEmpty()) return;

        var connections = _userConnectionManager.GetUserConnections(userId);
        if (connections is not {Count: > 0}) return;

        foreach (var connectionId in connections) _notification.Clients.Client(connectionId).SendMessage(message);
    }

    private static Guid GetUserId(ClaimsPrincipal principal)
    {
        return Guid.TryParse(principal.FindFirstValue(ClaimType.NameId), out var userId) ? userId : default;
    }

    #endregion
}