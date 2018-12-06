using Kelson.Common.Async;
using Kelson.Common.Events;
using Kelson.Common.Wpf;
using MyWpfApp.ClientInterface;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace MyWpfApp
{
    public class ChatroomViewModel : BaseViewModel
    {
        public event Action OnLogout;

        private readonly IChatClient client;

        public ObservableCollection<MessageViewModel> Messages { get; set; }
            = new ObservableCollection<MessageViewModel>();

        private string _textToSend;
        public string TextToSend { get => _textToSend; set => Set(ref _textToSend, value); }

        public ICommand LogoutCommand { get; set; }
        public ICommand SendMessageCommand { get; set; }

        private readonly IDispatcher uiThread;

        public ChatroomViewModel(IEventManager context) : base(context)
        {
            client = context.Request<IChatClient>();
            uiThread = context.Request<IDispatcher>();
            LogoutCommand = new ActionCommand(logout);
            SendMessageCommand = new ActionCommand(sendMessage);

            var trace = new StackTrace(0);
            client.OnMessageRecieved += message =>            
                uiThread.Dispatch(
                    () => RecieveMessage(message),
                    trace,
                    $"Client OnMessageRecieved dispatch in ctor of {nameof(ChatroomViewModel)}");
        }

        private void sendMessage()
        {
            var message = new ClientMessage { Text = TextToSend };            
            Messages.Add(new SentMessageViewModel(message));
            client.SendMessageAsync(message).ConfirmOn(uiThread, errorCallback: e => throw e);                
        }

        private void RecieveMessage(IMessage message)
        {
            var sent = Messages.FirstOrDefault(m => m.Id == message.Id);
            if (sent != null)
            {
                var index = Messages.IndexOf(sent);
                Messages.RemoveAt(index);
                Messages.Insert(index, new RecievedMessageViewModel(message));
            }
            else
            {
                Messages.Add(new RecievedMessageViewModel(message));
            }
        }

        private void logout()
        {
            OnLogout?.Invoke();
        }
    }
}
