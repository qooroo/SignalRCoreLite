using Infrastructure;

namespace Messages
{
    public class ClientPublisherMessage : IPayloadMessage<IClientPublisher>
    {
        public IClientPublisher Payload { get; }

        public ClientPublisherMessage(IClientPublisher payload)
        {
            Payload = payload;
        }
    }
}