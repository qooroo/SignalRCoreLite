using System;
using System.Buffers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Messages;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections.Features;

namespace SignalRCoreLite
{
    public class MessagesConnectionHandler : ConnectionHandler, IClientResponseGateway
    {
        private readonly string _id;
        private readonly Agent _agent;
        private readonly InstrumentationMessage _inst = new InstrumentationMessage();

        private ConnectionList Connections { get; } = new ConnectionList();

        public MessagesConnectionHandler(Agent agent)
        {
            _id = Guid.NewGuid().ToString();
            _agent = agent;
            _agent.Publish(new GatewayMessage(this));
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            System.Console.WriteLine($"OnConnected @ {_id}");

            Connections.Add(connection);

            var transportType = connection.Features.Get<IHttpTransportFeature>()?.TransportType;

            _agent.Publish(StringMessage.Create($"{connection.ConnectionId}>({transportType})"));

            try
            {
                while (true)
                {
                    var result = await connection.Transport.Input.ReadAsync();
                    var buffer = result.Buffer;

                    try
                    {
                        if (!buffer.IsEmpty)
                        {
                            var s = Encoding.UTF8.GetString(buffer.ToArray());

                            System.Console.WriteLine($"publishing message from {_id}");
                            _agent.Publish(StringMessage.Create($"{connection.ConnectionId}>{s}"));
                            // manual trigger of instrumentation
                            if (s == "i") _agent.Publish(_inst);
                        }
                        else if (result.IsCompleted)
                        {
                            break;
                        }
                    }
                    finally
                    {
                        connection.Transport.Input.AdvanceTo(buffer.End);
                    }
                }
            }
            finally
            {
                Connections.Remove(connection);

                _agent.Publish(StringMessage.Create($"{connection.ConnectionId}!>({transportType})"));
            }
        }

        public Task Broadcast(string message)
        {
           return Broadcast(Encoding.UTF8.GetBytes(message));
        }

        private Task Broadcast(byte[] payload)
        {
           return Task.WhenAll(Connections.Select(x => x.Transport.Output.WriteAsync(payload).AsTask()));
        }

        public Task Send(string connectionId, string message)
        {
            var connection = Connections[connectionId];
            return connection.Transport.Output.WriteAsync(Encoding.UTF8.GetBytes(message)).AsTask();
        }
    }
}
