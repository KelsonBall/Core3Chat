using Kelson.Common.Async;
using Kelson.Common.Events;
using Kelson.Common.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace MyWpfApp
{
    /// <summary>
    /// Allow connecting to multiple chatrooms
    /// </summary>
    public class ServerListViewModel : BaseViewModel
    {
        public ObservableCollection<ServerViewModel> ServerPages { get; set; }
         = new ObservableCollection<ServerViewModel>();
        
        private readonly IAppHost application;
        private readonly IDispatcher uiThread;

        private ServerViewModel _content;
        public ServerViewModel Content { get => _content; set => Set(ref _content, value); }

        public ICommand AddServerCommand { get; set; }

        public ServerListViewModel(IEventManager context) : base(context)
        {
            AddServerCommand = new ActionCommand(addServer);
        }

        private void addServer()
        {
            var connect = new ServerViewModel(context)
            {
                Title = "New Connection",
            };

            connect.OnClosed += () =>
            {
                if (ServerPages.Contains(connect))
                    ServerPages.Remove(connect);
            };

            connect.OnSelected += () => selectViewModel(connect);

            ServerPages.Add(connect);
            selectViewModel(connect);
        }

        private void selectViewModel(ServerViewModel vm)
        {
            if (Content != null)
                Content.IsSelected = false;
            vm.IsSelected = true;
            Content = vm;
        }

        private void CloseApplication()
            => application.CloseApplication();
    }
}
