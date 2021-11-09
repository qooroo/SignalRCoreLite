using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Infrastructure.Stats
{
    public record class WorkerStats
    {
        private static readonly double TicksPerMillisecond = Stopwatch.Frequency / 1000d;

        public WorkerStats(IReadOnlyCollection<MessageStat> stats)
        {
            MessageCount = stats.Count;
            if (stats.Count <= 0) return;
            
            MaxQueueDelayMs = Math.Round((from s in stats select s.SubmittedToStarted).Max() / TicksPerMillisecond, 3);
            AverageQueueDelayMs = Math.Round((from s in stats select s.SubmittedToStarted).Average() / TicksPerMillisecond, 3);
            MaxProcessingTimeMs = Math.Round((from s in stats select s.SubmittedToCompleted - s.SubmittedToStarted).Max() / TicksPerMillisecond, 3);
            AverageProcessingTimeMs = Math.Round((from s in stats select s.SubmittedToCompleted - s.SubmittedToStarted).Average() / TicksPerMillisecond, 3);
            MaxQueueLength = (from s in stats select s.QueueLength).Max();
        }

        public int MaxQueueLength { get; }

        public double AverageProcessingTimeMs { get; }

        public double MaxProcessingTimeMs { get; }

        public double AverageQueueDelayMs { get; }

        public double MaxQueueDelayMs { get; }

        public int MessageCount { get; }
    }
}
