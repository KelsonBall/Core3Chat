using Kelson.Common.Wpf;
using MyWpfApp.ClientInterface;
using MyWpfApp.SignalrClient;
using System;
using System.Windows.Input;

namespace MyWpfApp
{
    public class LoginViewModel : SimpleViewModel
    {
        public event Action<ICredential> OnLogin;

        private string _username;
        public string Username { get => _username; set => Set(ref _username, value); }

        private string _server = "localhost:5123";
        public string Server { get => _server; set => Set(ref _server, value); }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel() 
        {
            LoginCommand = new ActionCommand(Login);
        }

        private void Login()
        {
            if (string.IsNullOrEmpty(Username))
                return;
            else
                OnLogin?.Invoke(new Credential
                {
                    Username = Username,
                    Host = Server,
                    Token = Guid.NewGuid().ToString() // 🤔 💩
                });
        }
    }
}
