using Infrastructure;

namespace Messages
{
    public class ClientPublisherMessage : IMessage
    {
        
        public ClientPublisherMessage(IClientPublisher clientPublisher)
        {
            ClientPublisher = clientPublisher;
        }

        public IClientPublisher ClientPublisher { get; }
    }

    public class GatewayPublisherMessage
    {
        public IServicePublisher ServicePublisher { get; }

        public GatewayPublisherMessage(IServicePublisher servicePublisher)
        {
            ServicePublisher = servicePublisher;
        }
    }
}