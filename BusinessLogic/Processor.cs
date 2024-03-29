﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Stats;

namespace BusinessLogic
{
    public class Processor : IMessageProcessor, IStatsHandler
    {
        private IClientResponseGateway _clientResponseGateway;
        private Agent _agent;

        public void OnStats(WorkerStats s)
        {
            Console.WriteLine($"Msg count: {s.MessageCount} Av msg time: {s.AverageProcessingTimeMs} Max msg time: {s.MaxProcessingTimeMs}");
        }

        public void OnMessage(IMessage message)
        {
            Console.WriteLine($"Processing on thread {Thread.CurrentThread.Name}:${Thread.CurrentThread.ManagedThreadId}");

            switch (message){
                case StringMessage s:
                    Console.WriteLine($"Processing: {s.S}");
                    try
                    {
                        if (s.S.Split('>')[1] == "b")
                        {
                            _clientResponseGateway.Broadcast("hello everybody!");    
                        }
                        if (s.S.Split('>')[1] == "r")
                        {
                            _clientResponseGateway.Send(s.S.Split('>')[0], "reply just to you!");
                        }
                        if (s.S.Split('>')[1] == "tp")
                        {
                            Task.Run(() =>
                            {
                                Console.WriteLine($"Doing long running work on thread {Thread.CurrentThread.Name}");
                                Thread.Sleep(TimeSpan.FromSeconds(5));
                                var result = Guid.NewGuid().ToString()[..4];
                                Console.WriteLine($"Calculated long running result: {result}");
                                _agent.Publish(ThreadPoolProcessedMessage.Create(s.S.Split('>')[0], result));
                            });
                            _clientResponseGateway.Send(s.S.Split('>')[0], "processing message bound for the threadpool");
                        }
                    }
                    catch (Exception ex) {
                        Console.WriteLine("BOOM!");
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case ThreadPoolProcessedMessage tp:
                    Console.WriteLine($"Received result {tp.S} on thread {Thread.CurrentThread.Name}");
                    _clientResponseGateway.Send(tp.ReplyTo, $"Threadpool processed result: {tp.S}");
                    break;
            }
        }

        public void InitPublisher(Agent agent)
        {
            _agent = agent;
        }

        public void RegisterResponseChannel(IClientResponseGateway gateway)
        {
            _clientResponseGateway = gateway;
        }
    }
}
