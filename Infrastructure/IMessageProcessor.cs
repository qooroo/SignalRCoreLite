namespace Infrastructure
{
    public interface IMessageProcessor
    {
        void OnMessage(IMessage message);
    }
}
