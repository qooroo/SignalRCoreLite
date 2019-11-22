using System;

namespace Infrastructure.Stats
{
    public class MessageStat
    {
        public Type MessageType { get; set; }
        public long SubmittedToStarted { get; set; }
        public long SubmittedToCompleted { get; set; }
        public int QueueLength { get; set; }
    }
}
