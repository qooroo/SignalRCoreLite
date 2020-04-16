namespace Infrastructure
{
    public interface IBusinessLogicProcessor : IMessageProcessor, IStatsHandler
    {
        void InitPublisher(Agent agent);
    }
}