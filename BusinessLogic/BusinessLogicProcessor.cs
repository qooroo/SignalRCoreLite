using System;
using System.Threading;
using Infrastructure;
using Infrastructure.Stats;
using Messages;

namespace BusinessLogic
{

    public class BusinessLogicProcessor : IBusinessLogicProcessor
    {
        private IServicePublisher _gatewayPubilsher;
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
                    break;
                case GatewayPublisherMessage g:
                    _gatewayPubilsher = g.ServicePublisher;
                    break;
                case ThreadPoolProcessedMessage tp:
                    Console.WriteLine($"Received result {tp.S} on thread {Thread.CurrentThread.Name}");
                    _gatewayPubilsher.Send(tp.ReplyTo, $"Threadpool processed result: {tp.S}");
                    break;
            }
        }

        public void InitPublisher(Agent agent)
        {
            _agent = agent;
        }
    }
}
