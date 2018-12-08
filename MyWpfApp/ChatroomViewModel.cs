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

        public event Action OnLostConnection;

        private readonly IChatClient client;

        public ObservableCollection<MessageViewModel> Messages { get; set; }
            = new ObservableCollection<MessageViewModel>();

        private string _textToSend;
        public string TextToSend { get => _textToSend; set => Set(ref _textToSend, value); }

        public ICommand LogoutCommand { get; set; }
        public ICommand SendMessageCommand { get; set; }

        private readonly IDispatcher uiThread;
        private readonly IAppHost application;

        public ChatroomViewModel(IEventManager context, IChatClient client) : base(context)
        {
            this.client = client;
            uiThread = context.Request<IDispatcher>();
            application = context.Request<IAppHost>();

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
            if (!string.IsNullOrEmpty(TextToSend))
            {
                var message = new ClientMessage { Text = TextToSend };
                Messages.Add(new SentMessageViewModel(message));
                TextToSend = null;
                client.SendMessageAsync(message).ConfirmOn(uiThread, errorCallback: handleNetworkError);
            }
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

        const string CONNECTION_NOT_ACTIVE = "connection is not active";

        private void handleNetworkError(Exception e)
        {
            if (e.Message.Contains(CONNECTION_NOT_ACTIVE))
            {
                application.WarnUser("Connection to server lost 🙁", "NETWORK ERROR");
                OnLostConnection?.Invoke();
            }
            else
                throw e;
        }
    }
}
