using System.Diagnostics;

namespace Infrastructure.Stats
{
    internal class AgentItem
    {
        public AgentItem(IMessage message)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            Type = message.GetType().Name;
            _stat = new MessageStat();
            Message = message;
        }

        public IMessage Message { get; }
        public string Type { get; }

        private readonly MessageStat _stat;
        private readonly Stopwatch _stopwatch;

        public void MarkStarted()
        {
            _stat.SubmittedToStarted = _stopwatch.ElapsedTicks;
        }

        public void MarkDone()
        {
            _stat.SubmittedToCompleted = _stopwatch.ElapsedTicks;
            _stopwatch.Stop();
        }

        public MessageStat GetStat()
        {
            return _stat;
        }

        public void SetQueueLength(int count)
        {
            _stat.QueueLength = count;
        }
    }
}
