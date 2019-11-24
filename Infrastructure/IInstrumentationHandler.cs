using Infrastructure.Stats;

namespace Infrastructure
{
    public interface IInstrumentationHandler
    {
        void OnInstrumentation(AgentStatistics agentStatistics);
    }
}
