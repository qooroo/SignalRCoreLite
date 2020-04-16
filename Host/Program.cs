using BusinessLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Gateway;
using Infrastructure;

namespace Host
{
    public static class Program
    {
        private const string Gateway = "gateway";
        private const string Cluster = "cluster";

        private static Dictionary<string, dynamic> LaunchProperties = new Dictionary<string, dynamic>
        {
            {Gateway, new {port = 5000}},
            {Cluster, new {port = 6000}}
        };

        public static void Main(string[] args)
        {
            switch (args[0])
            {
                case Gateway:
                    Launch(new GatewayProcessor(), LaunchProperties[Gateway]);
                    break;
                case Cluster:
                    Launch(new BusinessLogicProcessor(),  LaunchProperties[Cluster]);
                    break;
                default:
                    Console.WriteLine("\nRun with args: gateway|cluster\n");
                    break;
            }
        }

        private static void Launch(IBusinessLogicProcessor p, dynamic launchProperties)
        {
            var agent = new Agent(p.GetType().Name, p, null);
            p.InitPublisher(agent);

            var host = new WebHostBuilder()
                .ConfigureLogging(factory => factory.AddConsole())
                .UseKestrel(options => options.ListenLocalhost(launchProperties.port))
                .ConfigureServices(services =>
                {
                    services.AddSingleton(p.GetType(), p);
                    services.AddSingleton(typeof(Agent), agent);
                    services.AddSignalR(options => options.KeepAliveInterval = TimeSpan.FromSeconds(5));
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapConnectionHandler<MessagesConnectionHandler>("/ingress"));
                })
                .Build();

            agent.Start();
            host.Run();
        }
    }
}