using System.Windows;

namespace MyWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly CoreAppShell shell;

        public App() => shell = new CoreAppShell(this);
    }
}
