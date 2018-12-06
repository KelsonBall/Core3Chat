using System;
using System.Threading.Tasks;

namespace MyWpfApp.ClientInterface
{
    public interface IChatClient
    {
        Task SendMessageAsync(IMessage message);
        void RecieveMessage(IMessage message);
        event Action<IMessage> OnMessageRecieved;
        Task DisposeAsync();
    }
}
