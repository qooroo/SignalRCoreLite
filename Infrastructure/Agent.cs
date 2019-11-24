using Infrastructure.Stats;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Infrastructure
{
    public class Agent : IAgent
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly IErrorHandler _errorHandler;
        private readonly BlockingCollection<AgentItem> _queue = new BlockingCollection<AgentItem>();
        private readonly StatBuffer<MessageStat> _stats = new StatBuffer<MessageStat>(5000);

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
                Publish(new PoisonPillMessage());
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


                    if (item.Type == nameof(PoisonPillMessage))
                    {
                        _running = false;
                    }
                    else if (item.Type == nameof(InstrumentationMessage))
                    {
                        var instrumentedMessageProcessor = _messageProcessor as IInstrumentationHandler;
                        instrumentedMessageProcessor?.OnInstrumentation(new AgentStatistics(_stats.GetBatch()));
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
            _queue.Add(new AgentItem(message));
        }
    }
}
