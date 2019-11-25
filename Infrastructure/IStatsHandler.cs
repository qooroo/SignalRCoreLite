using Infrastructure.Stats;

namespace Infrastructure
{
    public interface IStatsHandler
    {
        void OnStats(WorkerStats agentStatistics);
    }
}
