using Kelson.Common.Wpf;
using MyWpfApp.ClientInterface;
using System;

namespace MyWpfApp
{
    public class MessageViewModel : SimpleViewModel, IMessage
    {
        private string _text;
        public string Text { get => _text; set => Set(ref _text, value); }

        public string Id { get; set; }

        private string _username;
        public string Username { get => _username; set => Set(ref _username, value); }

        public DateTimeOffset TimeSentFromServer { get; set; }

        public MessageViewModel(IMessage message)
        {
            Text = message.Text;
            Id = message.Id;
            Username = message.Username;
            TimeSentFromServer = message.TimeSentFromServer;
        }
    }

    public class RecievedMessageViewModel : MessageViewModel
    {
        public RecievedMessageViewModel(IMessage message) : base(message) { }
    }

    public class SentMessageViewModel : MessageViewModel
    {
        public SentMessageViewModel(IMessage message) : base(message) { }
    }


}
