using BusinessLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Infrastructure;

namespace Host
{
    public static class Program
    {
        public static void Main()
        {
            var p = new Processor();
            var agent = new Agent("Model", p, null);
            var messagesConnectionHandler = new MessagesConnectionHandler(agent);
            
            p.RegisterResponseChannel(messagesConnectionHandler);
            p.InitPublisher(agent);

            var host = new WebHostBuilder()
                .ConfigureLogging(factory => factory.AddConsole())
                .UseKestrel(options => options.ListenLocalhost(5000))
                .ConfigureServices(services =>{
                    services.AddSingleton(typeof(Processor), p);
                    services.AddSingleton(typeof(Agent), agent);
                    services.AddSingleton(typeof(MessagesConnectionHandler), messagesConnectionHandler);
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
