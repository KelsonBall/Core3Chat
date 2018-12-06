using Kelson.Common.Async;
using Kelson.Common.Events;
using Kelson.Common.Wpf;
using MyWpfApp.ClientInterface;
using MyWpfApp.SignalrClient;
using System.ComponentModel;
using System.Windows.Input;

namespace MyWpfApp
{
    public class MainWindowViewModel : BaseViewModel
    {

        private readonly LoginViewModel loginVm;        
        private ChatroomViewModel chatVm;

        private INotifyPropertyChanged _content;
        public INotifyPropertyChanged Content { get => _content; set => Set(ref _content, value); }

        private readonly IAppHost application;
        private readonly IDispatcher uiThread;

        public ICommand CloseApplicationCommand { get; set; }

        public MainWindowViewModel(IEventManager context) : base(context)
        {
            application = context.Request<IAppHost>();
            uiThread = context.Request<IDispatcher>();

            CloseApplicationCommand = new ActionCommand(CloseApplication);
            SignalrChatClient client = null;
            loginVm = new LoginViewModel();
            loginVm.OnLogin += username =>
            {
                if (client != null)
                    client
                        .DisposeAsync()
                        .ConfirmOn(uiThread, errorCallback: e => throw e);
                client = new SignalrChatClient(username);
                client.StartAsync().ConfirmOn(uiThread, errorCallback: e => throw e);
                context.Provide<IChatClient>(() => client);
                chatVm = new ChatroomViewModel(context);
                chatVm.OnLogout += () =>
                {
                    Content = loginVm;
                };
                Content = chatVm;

            };

            Content = loginVm;
        }

        private void CloseApplication()
            => application.CloseApplication();
    }
}
