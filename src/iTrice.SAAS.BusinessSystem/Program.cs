using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace iTrice.SAAS.BusinessSystem
{
    public class Program
    {
        public static string SelfConfig {get;set;}

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (args != null && args.Length > 1)
            {
                SelfConfig = args[1];
                return WebHost.CreateDefaultBuilder(args).UseUrls(args[0]).UseStartup<Startup>();
            }
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}
