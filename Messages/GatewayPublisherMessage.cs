using Infrastructure;

namespace Messages
{
    public class GatewayPublisherMessage : IPayloadMessage<IServicePublisher>
    {
        public IServicePublisher Payload { get; }
        
        public GatewayPublisherMessage(IServicePublisher servicePublisher)
        {
            Payload = servicePublisher;
        }
    }
}