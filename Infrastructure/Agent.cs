using Infrastructure.Stats;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Infrastructure
{
    public class Agent : IWorker
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly IErrorHandler _errorHandler;
        private readonly BlockingCollection<WorkerItem> _queue = new();
        private readonly StatBuffer<MessageStat> _stats = new(5000);

        private Thread _thread;
        private volatile bool _running;

        public string Name { get; }

        public Agent(string name, IMessageProcessor messageProcessor, IErrorHandler errorHandler)
        {
            _messageProcessor = messageProcessor;
            _errorHandler = errorHandler;

            Name = name;
        }

        public void Start()
        {
            _running = true;
            _thread = new Thread(Run) { Name = Name };
            _thread.Start();
        }

        public void Stop()
        {
            if (_running)
            {
                Publish(new KillMessage());
                _thread.Join(1000);
            }
        }

        private void Run()
        {
            while (_running)
            {
                try
                {
                    var item = _queue.Take();

                    item.SetQueueLength(_queue.Count);
                    item.MarkStarted();


                    if (item.Type == nameof(KillMessage))
                    {
                        _running = false;
                    }
                    else if (item.Type == nameof(StatsMessage))
                    {
                        var instrumentedMessageProcessor = _messageProcessor as IStatsHandler;
                        instrumentedMessageProcessor?.OnStats(new WorkerStats(_stats.GetBatch()));
                    }
                    else
                    {
                        _messageProcessor.OnMessage(item.Message);
                    }

                    item.MarkDone();
                    var messageStat = item.GetStat();
                    _stats.Add(messageStat);
                }
                catch (Exception exception)
                {
                    var shouldExit = _errorHandler.OnError(Name, exception);
                    _running = !shouldExit;
                }
            }
        }

        public void Publish(IMessage message)
        {
            _queue.Add(new WorkerItem(message));
        }
    }
}
