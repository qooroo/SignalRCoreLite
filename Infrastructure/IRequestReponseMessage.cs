namespace Infrastructure
{
    public interface IRequestReponseMessage : IMessage
    {
        string ReplyTo { get; }
    }
}