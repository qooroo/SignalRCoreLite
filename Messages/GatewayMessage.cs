using Infrastructure;

namespace Messages
{
    public class GatewayMessage : IMessage
    {
        public GatewayMessage(IClientResponseGateway clientResponseGateway)
        {
            ClientResponseGateway = clientResponseGateway;
        }

        public IClientResponseGateway ClientResponseGateway { get; }
    }
}