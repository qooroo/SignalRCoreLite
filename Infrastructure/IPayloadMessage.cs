namespace Infrastructure
{
    public interface IPayloadMessage<out T> : IMessage
    {
        T Payload { get; }
    }
}