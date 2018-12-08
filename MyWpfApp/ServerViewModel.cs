using Kelson.Common.Async;
using Kelson.Common.Events;
using Kelson.Common.Wpf;
using MyWpfApp.ClientInterface;
using MyWpfApp.SignalrClient;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace MyWpfApp
{
    /// <summary>
    /// Show login page or active chat room
    /// </summary>
    public class ServerViewModel : BaseViewModel
    {
        public event Action OnClosed;
        public event Action OnSelected;

        private string _title;
        public string Title { get => _title; set => Set(ref _title, value); }

        private bool _isSelected;
        public bool IsSelected { get => _isSelected; set => Set(ref _isSelected, value); }

        private readonly LoginViewModel loginVm;
        private ChatroomViewModel chatVm;

        private INotifyPropertyChanged _content;
        public INotifyPropertyChanged Content { get => _content; set => Set(ref _content, value); }
        
        private readonly IDispatcher uiThread;        

        public ICommand SelectCommand { get; set; }

        public ServerViewModel(IEventManager context) : base(context)
        {            
            uiThread = context.Request<IDispatcher>();
            
            SignalrChatClient client = null;
            loginVm = new LoginViewModel();
            loginVm.OnLogin += credentials =>
            {
                Title = $"{credentials.Username}@{credentials.Host}";
                if (client != null)
                    client
                        .DisposeAsync()
                        .ConfirmOn(uiThread, errorCallback: e => throw e);                
                client = new SignalrChatClient(credentials);
                client.StartAsync().ConfirmOn(uiThread, errorCallback: e => throw e);               
                chatVm = new ChatroomViewModel(context, client);
                chatVm.OnLogout += () => Content = loginVm;                
                chatVm.OnLostConnection += () => OnClosed?.Invoke();
                Content = chatVm;
            };

            Content = loginVm;

            SelectCommand = new ActionCommand(() => OnSelected?.Invoke());
        }

    }
}
