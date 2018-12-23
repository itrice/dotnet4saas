using System;
using iTrice.SAAS.TenantManager.Data;
using iTrice.SAAS.TenantManager.Server;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace iTrice.SAAS.TenantManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var servicePrivider = scope.ServiceProvider;
                var content = servicePrivider.GetService<TenantContext>();
                DbInitializer.Initialize(content);
                //Console.WriteLine("Initialize db");
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}
