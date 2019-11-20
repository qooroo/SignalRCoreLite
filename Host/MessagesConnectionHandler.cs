using System.Buffers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections.Features;

namespace SignalRCoreLite
{
    public class MessagesConnectionHandler : ConnectionHandler
    {
        private readonly Ingress _ingress;

        private ConnectionList Connections { get; } = new ConnectionList();

        public MessagesConnectionHandler(Ingress ingress)
        {
            _ingress = ingress;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            Connections.Add(connection);

            var transportType = connection.Features.Get<IHttpTransportFeature>()?.TransportType;

            _ingress.OnMessage($"{connection.ConnectionId} connected ({transportType})");

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
                            _ingress.OnMessage($"{connection.ConnectionId} sent {s}");
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

                _ingress.OnMessage($"{connection.ConnectionId} disconnected ({transportType})");
            }
        }

        //private Task Broadcast(string text)
        //{
        //    return Broadcast(Encoding.UTF8.GetBytes(text));
        //}

        //private Task Broadcast(byte[] payload)
        //{
        //    return Task.WhenAll(Connections.Select(x => x.Transport.Output.WriteAsync(payload).AsTask()));
        //}
    }
}
