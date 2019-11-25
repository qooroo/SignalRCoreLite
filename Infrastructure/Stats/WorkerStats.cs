using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Infrastructure.Stats
{
    public class WorkerStats
    {
        static readonly double TicksPerMillisecond = Stopwatch.Frequency / 1000d;

        public WorkerStats(List<MessageStat> stats)
        {
            MessageCount = stats.Count;
            if (stats.Count > 0)
            {
                MaxQueueDelayMs = Math.Round((from s in stats select s.SubmittedToStarted).Max() / TicksPerMillisecond, 3);
                AverageQueueDelayMs = Math.Round((from s in stats select s.SubmittedToStarted).Average() / TicksPerMillisecond, 3);
                MaxProcessingTimeMs = Math.Round((from s in stats select s.SubmittedToCompleted - s.SubmittedToStarted).Max() / TicksPerMillisecond, 3);
                AverageProcessingTimeMs = Math.Round((from s in stats select s.SubmittedToCompleted - s.SubmittedToStarted).Average() / TicksPerMillisecond, 3);
                MaxQueueLength = (from s in stats select s.QueueLength).Max();
            }
        }

        public int MaxQueueLength { get; set; }

        public double AverageProcessingTimeMs { get; set; }

        public double MaxProcessingTimeMs { get; set; }

        public double AverageQueueDelayMs { get; set; }

        public double MaxQueueDelayMs { get; set; }

        public int MessageCount { get; set; }
    }
}
