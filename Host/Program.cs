using BusinessLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

using SignalRCoreLite;
using System;
using Infrastructure;
using Microsoft.AspNetCore.Connections;

namespace Host
{
    public static class Program
    {
        public static void Main()
        {
            var p = new Processor();
            var agent = new Agent("Model", p, null);

            var host = new WebHostBuilder()
                .ConfigureLogging(factory => factory.AddConsole())
                .UseKestrel(options => options.ListenLocalhost(5000))
                .ConfigureServices(services =>{
                    services.AddSingleton(typeof(Processor), p);
                    services.AddSingleton(typeof(Agent), agent);
                    services.AddSignalR(options => options.KeepAliveInterval = TimeSpan.FromSeconds(5));
                })
                .Configure(app => {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapConnectionHandler<MessagesConnectionHandler>("/ingress"));
                })
                .Build();

            agent.Start();

            host.Run();
        }
    }
}
