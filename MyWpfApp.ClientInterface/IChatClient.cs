using System;
using System.Threading.Tasks;

namespace MyWpfApp.ClientInterface
{
    public interface ICredential
    {
        string Username { get; set; }
        string Host { get; set; }
        string Token { get; set; }
    }

    public interface IChatClient
    {
        Task SendMessageAsync(IMessage message);
        void RecieveMessage(IMessage message);
        event Action<IMessage> OnMessageRecieved;
        Task DisposeAsync();
    }
}
