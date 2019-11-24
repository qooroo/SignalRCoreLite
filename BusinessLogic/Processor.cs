using System;
using System.Threading;
using Infrastructure;
using Infrastructure.Stats;
using Messages;

namespace BusinessLogic
{
    public class Processor : IMessageProcessor, IInstrumentationHandler
    {
        private IClientResponseGateway _clientResponseGateway;

        public void OnInstrumentation(AgentStatistics s)
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
                            _clientResponseGateway.Broadcast("heelo everybody!");    
                        }
                        if (s.S.Split('>')[1] == "r")
                        {
                            _clientResponseGateway.Send(s.S.Split('>')[0], "reply just to you!");
                        }
                    }
                    catch {
                        Console.WriteLine("BOOM!");
                    }
                    break;
                case GatewayMessage g:
                    _clientResponseGateway = g.ClientResponseGateway;
                    break;
            }
        }
    }
}
