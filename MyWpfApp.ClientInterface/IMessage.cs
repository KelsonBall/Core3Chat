using System;

namespace MyWpfApp.ClientInterface
{
    public interface IMessage
    {
        string Id { get; set; }
        string Username { get; set; }
        string Text { get; set; }
        DateTimeOffset TimeSentFromServer { get; set; }
    }

    public class ClientMessage : IMessage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Username { get; set; }

        public string Text { get; set; }

        private readonly DateTimeOffset _timeObjectCreated = DateTimeOffset.Now;
        public DateTimeOffset TimeSentFromServer { get => _timeObjectCreated; set { } }
    }

    public class ServerMessage : IMessage
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }
        
        public DateTimeOffset TimeSentFromServer { get; set; }
    }
}
