using Infrastructure;

namespace Messages
{
    public class StringMessage : IRequestReponseMessage, IPayloadMessage<string>
    {
        public string ReplyTo { get; }
        public string Payload { get; }

        public StringMessage(string replyTo, string payload)
        {
            ReplyTo = replyTo;
            Payload = payload;
        }
    }
}