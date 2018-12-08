using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MyWpfApp.ClientInterface;

namespace MyWpfApp.Server
{
    public class ChatHub : Hub, IChatServer<ServerMessage>
    {
        protected static readonly ConcurrentQueue<ServerMessage> messages =
            new ConcurrentQueue<ServerMessage>();

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Connection from {Clients.Caller.ToString()}");
            foreach (var message in messages)
                await Clients.Caller.SendAsync(nameof(IChatClient.RecieveMessage), message);
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(ServerMessage message)
        {
            message.TimeSentFromServer = DateTimeOffset.Now;
            Console.WriteLine();
            Console.WriteLine(message.Username);
            Console.WriteLine(message.Text);

            messages.Enqueue(message);
            while (messages.Count > 10)
                messages.TryDequeue(out _);            

            await Clients.All.SendAsync(nameof(IChatClient.RecieveMessage), message);
        }
    }
}
