using System;
using System.Buffers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections.Features;

namespace Infrastructure
{
    public class MessagesConnectionHandler : ConnectionHandler, IClientResponseGateway
    {
        private readonly string _id;
        private readonly Agent _agent;
        private readonly StatsMessage _inst = new();

        private ConnectionList Connections { get; } = new();

        public MessagesConnectionHandler(Agent agent)
        {
            _id = Guid.NewGuid().ToString();
            _agent = agent;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            Console.WriteLine($"OnConnected @ {_id}");

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

                            Console.WriteLine($"publishing message from {_id}");
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

        public Task Broadcast(string message) => Broadcast(Encoding.UTF8.GetBytes(message));

        private Task Broadcast(byte[] payload) => Task.WhenAll(Connections.Select(x => x.Transport.Output.WriteAsync(payload).AsTask()));

        public Task Send(string connectionId, string message)
        {
            var connection = Connections[connectionId];
            return connection.Transport.Output.WriteAsync(Encoding.UTF8.GetBytes(message)).AsTask();
        }
    }
}
