using MyWpfApp.ClientInterface;

namespace MyWpfApp.SignalrClient
{
    public class Credential : ICredential
    {
        public string Username { get; set; }
        public string Host { get; set; }
        public string Token { get; set; }
    }
}
