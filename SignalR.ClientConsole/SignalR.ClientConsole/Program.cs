using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Net.Http.Json;

using SignalR.ClientConsole;

HttpClient client = new();

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7171/signalR/notification", options =>
    {
        options.AccessTokenProvider = async () =>
        {
            var loginRequest = new LoginRequestModel()
            {
                Username = "admin",
                Password = "admin"
            };

            var token = await GetAccessTokenAsync(loginRequest);
            Console.WriteLine(JsonConvert.SerializeObject(token, JsonSettings()));
            return token.AccessToken;
        };

    })
    .Build();

connection.On<object>("SendMessage", (message) =>
{
    Console.WriteLine(message);
});

// Loop is here to wait until the server is running
while (true)
{
    try
    {
        await connection.StartAsync();

        Console.WriteLine(connection.ConnectionId);

        break;
    }
    catch (Exception exception)
    {
        Console.WriteLine(exception.Message);
        await Task.Delay(1000);
    }
}

Console.WriteLine("Client listening eHubb SignalR Message. Hit Ctrl-C to quit.");
Console.ReadLine();


JsonSerializerSettings JsonSettings()
{
    return new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Formatting = Newtonsoft.Json.Formatting.None,
        Culture = CultureInfo.CurrentUICulture

    };
}

async Task<AccessTokenModel> GetAccessTokenAsync(LoginRequestModel login)
{
    Console.WriteLine("GetAccessTokenAsync");
    HttpResponseMessage response = await client.PostAsJsonAsync(
        "https://localhost:7171/Authenticate", login);
    response.EnsureSuccessStatusCode();

    return await response.Content.ReadFromJsonAsync<AccessTokenModel>();
}