namespace Infrastructure
{
    public class ThreadPoolProcessedMessage : IMessage
    {
        public string S { get; }

        public string ReplyTo { get; }

        public static ThreadPoolProcessedMessage Create(string replyTo, string s)
        {
            return new ThreadPoolProcessedMessage(replyTo, s);
        }

        private ThreadPoolProcessedMessage(string replyTo, string s)
        {
            ReplyTo = replyTo;
            S = s;
        }
    }
}