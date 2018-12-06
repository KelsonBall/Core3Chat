using Kelson.Common.Events;
using Kelson.Common.Wpf;
using Kelson.Common.Wpf.Converters;
using Kelson.Common.Wpf.NetCore;
using System.Collections.Generic;

namespace MyWpfApp
{
    public class CoreAppShell : BaseShell
    {
        public CoreAppShell(System.Windows.Application application) : base(application)
        {
            var app = new CoreAppHost(() => application.MainWindow);
            context.Provide<IAppHost>(() => app);

            var window = new MainWindow
            {
                DataContext = new MainWindowViewModel(context)
            };            

            window.Show();
        }

        protected override IEventManager BuildApplicationContext()
        {
            return new EventManager();
        }

        protected override IEnumerable<BaseModule> GetBaseModules()
        {
            yield break;
        }

        protected override IEnumerable<ICrossPlatformValueConverter> GetConverters()
        {
            yield break;
        }
    }
}
