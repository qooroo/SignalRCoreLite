using System;
using BusinessLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRCoreLite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(Ingress));
            services.AddSingleton(typeof(Processor));
            services.AddSignalR(options => options.KeepAliveInterval = TimeSpan.FromSeconds(5));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapConnectionHandler<MessagesConnectionHandler>("/ingress"));
        }
    }
}
