using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.CancellationToken;

namespace TestClient
{
    class Program
    {
        public static async Task Main()
        {
            var c = new ClientWebSocket();
            
            await c.ConnectAsync(new Uri("ws://localhost:5000/ingress"), None);

            await c.Send("hey");

            Console.ReadLine();
        }
    }

    public static class Extensions
    {
        public static ReadOnlyMemory<byte> ToReadOnlyMemory(this string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            return new ReadOnlyMemory<byte>(bytes);
        }

        public static ValueTask Send(this ClientWebSocket client, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            return client.SendAsync(new ReadOnlyMemory<byte>(bytes), WebSocketMessageType.Binary, true, None);
        }
    }
}