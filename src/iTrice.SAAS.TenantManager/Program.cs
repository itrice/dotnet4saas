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
                //var content = servicePrivider.GetService<TenantContext>();
                //DbInitializer.Initialize(content);
                //Console.WriteLine("Initialize db");
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseUrls("http://192.168.1.126:9090").UseStartup<Startup>();
    }
}
