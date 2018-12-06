using Kelson.Common.Wpf;
using System;
using System.Windows.Input;

namespace MyWpfApp
{
    public class LoginViewModel : SimpleViewModel
    {
        public event Action<string> OnLogin;

        private string _username;
        public string Username { get => _username; set => Set(ref _username, value); }

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
                OnLogin?.Invoke(Username);
        }
    }
}
