using Infrastructure;

namespace Messages
{
    public class ThreadPoolProcessedMessage : IMessage
    {
        private readonly string _s;
        private readonly string _replyTo;
        public string S => _s;

        public string ReplyTo => _replyTo;

        public static ThreadPoolProcessedMessage Create(string replyTo, string s)
        {
            return new ThreadPoolProcessedMessage(replyTo, s);
        }

        private ThreadPoolProcessedMessage(string replyTo, string s)
        {
            _replyTo = replyTo;
            _s = s;
        }
    }
}