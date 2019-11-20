using BusinessLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace SignalRCoreLite
{
    public class Program
    {
        public static void Main()
        {
            var host = new WebHostBuilder()
                .ConfigureLogging(factory => factory.AddConsole())
                .UseKestrel(options => options.ListenLocalhost(5000))
                .UseStartup<Startup>()
                .Build();

            var p = host.Services.GetService(typeof(Processor)) as Processor;
            p.Run();

            host.Run();
        }
    }
}
