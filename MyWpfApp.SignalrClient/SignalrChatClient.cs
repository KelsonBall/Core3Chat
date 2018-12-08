using Microsoft.AspNetCore.SignalR.Client;
using MyWpfApp.ClientInterface;
using System;
using System.Threading.Tasks;

namespace MyWpfApp.SignalrClient
{
    public class SignalrChatClient : IChatClient
    {
        private readonly HubConnection hub;
        private readonly ICredential credentials;

        public SignalrChatClient(ICredential user)
        {
            hub = new HubConnectionBuilder()
                .WithUrl($"http://{user.Host}/chat/")
                .Build();
            this.credentials = user;
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
            message.Username = credentials.Username;
            await hub.SendAsync(nameof(IChatServer<ServerMessage>.SendMessage), message);
        }
    }
}
