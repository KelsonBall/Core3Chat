using Microsoft.AspNetCore.SignalR.Client;
using MyWpfApp.ClientInterface;
using System;
using System.Threading.Tasks;

namespace MyWpfApp.SignalrClient
{
    public class SignalrChatClient : IChatClient
    {
        private readonly HubConnection hub;
        private readonly string user;

        public SignalrChatClient(string user)
        {
            hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chat/")
                .Build();            
            this.user = user;
            hub.On<ServerMessage>(nameof(IChatClient.RecieveMessage), RecieveMessage);            
        }

        public async Task StartAsync() => await hub.StartAsync();

        public event Action<IMessage> OnMessageRecieved;

        public async Task DisposeAsync()
        {
            await hub.DisposeAsync();
        }

        public void RecieveMessage(IMessage message)
        {
            OnMessageRecieved?.Invoke(message);
        }

        public async Task SendMessageAsync(IMessage message)
        {
            message.Username = user;
            await hub.SendAsync(nameof(IChatServer<ServerMessage>.SendMessage), message);
        }
    }
}
