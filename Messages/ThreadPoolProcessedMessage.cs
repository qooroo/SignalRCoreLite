using Infrastructure;

namespace Messages
{
    public class ThreadPoolProcessedMessage<T> : IMessage<T>
    {
        public string ReplyTo { get; }
        public T Payload { get; }

        public ThreadPoolProcessedMessage(string replyTo, T payload)
        {
            ReplyTo = replyTo;
            Payload = payload;
        }
    }
}