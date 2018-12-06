using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MyWpfApp.ClientInterface;

namespace MyWpfApp.Server
{
    public class ChatHub : Hub, IChatServer<ServerMessage>
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connection from {Clients.Caller.ToString()}");
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(ServerMessage message)
        {
            message.TimeSentFromServer = DateTimeOffset.Now;
            Console.WriteLine();
            Console.WriteLine(message.Username);
            Console.WriteLine(message.Text);
            await Clients.All.SendAsync(nameof(IChatClient.RecieveMessage), message);
        }
    }
}
