using System.Threading.Tasks;

namespace MyWpfApp.ClientInterface
{
    public interface IChatServer<TMessage> where TMessage : IMessage
    {
        Task SendMessage(TMessage message);
    }
}
