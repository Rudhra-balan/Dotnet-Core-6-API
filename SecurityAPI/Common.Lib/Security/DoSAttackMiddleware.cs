using System.Net;
using System.Timers;
using Common.Lib.ResponseHandler.Resources;
using Microsoft.AspNetCore.Http;
using Timer = System.Timers.Timer;

namespace Common.Lib.Security;

public sealed class DosAttackMiddleware
{
    #region Private fields

    private static readonly Dictionary<string, short> IpAdresses = new();
    private static readonly Stack<string> Banned = new();
    private static Timer _timer = CreateTimer();
    private static Timer _bannedTimer = CreateBanningTimer();

    #endregion

    private
        const int BannedRequests = 10;

    private
        const int ReductionInterval = 1000; // 1 second    

    private
        const int ReleaseInterval = 5 * 60 * 1000; // 5 minutes    

    private readonly RequestDelegate _next;

    public DosAttackMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Connection.RemoteIpAddress != null)
        {
            var ip = httpContext.Connection.RemoteIpAddress.ToString();
            if (Banned.Contains(ip))
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                httpContext.Response.Body.SetLength(0);
                var message = ResponseMessage.AccessViolationException.message;
                await httpContext.Response.WriteAsync(message);
            }

            CheckIpAddress(ip);
        }

        await _next(httpContext);
    }

    /// <summary>
    ///     Checks the requesting IP address in the collection
    ///     and bannes the IP if required.
    /// </summary>
    private static void CheckIpAddress(string ip)
    {
        if (!IpAdresses.ContainsKey(ip))
        {
            IpAdresses[ip] = 1;
        }
        else if (IpAdresses[ip] == BannedRequests)
        {
            Banned.Push(ip);
            IpAdresses.Remove(ip);
        }
        else
        {
            IpAdresses[ip]++;
        }
    }

    #region Timers

    /// <summary>
    ///     Creates the timer that substract a request
    ///     from the _IpAddress dictionary.
    /// </summary>
    private static Timer CreateTimer()
    {
        var timer = GetTimer(ReductionInterval);
        timer.Elapsed += TimerElapsed;
        return timer;
    }

    /// <summary>
    ///     Creates the timer that removes 1 banned IP address
    ///     everytime the timer is elapsed.
    /// </summary>
    /// <returns></returns>
    private static Timer CreateBanningTimer()
    {
        var timer = GetTimer(ReleaseInterval);
        timer.Elapsed += delegate
        {
            if (Banned.Any()) Banned.Pop();
        };
        return timer;
    }

    /// <summary>
    ///     Creates a simple timer instance and starts it.
    /// </summary>
    /// <param name="interval">The interval in milliseconds.</param>
    private static Timer GetTimer(int interval)
    {
        var timer = new Timer {Interval = interval};
        timer.Start();
        return timer;
    }

    /// <summary>
    ///     Substracts a request from each IP address in the collection.
    /// </summary>
    private static void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        foreach (var key in IpAdresses.Keys.ToList())
        {
            IpAdresses[key]--;
            if (IpAdresses[key] == 0) IpAdresses.Remove(key);
        }
    }

    #endregion
}