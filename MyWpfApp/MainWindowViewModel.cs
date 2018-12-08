using Kelson.Common.Events;
using Kelson.Common.Wpf;

namespace MyWpfApp
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ServerListViewModel _servers;
        public ServerListViewModel Servers { get => _servers; set => Set(ref _servers, value); }

        public MainWindowViewModel(IEventManager context) : base(context)
        {
            Servers = new ServerListViewModel(context);
        }

    }
}
