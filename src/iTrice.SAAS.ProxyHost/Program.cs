using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace iTrice.SAAS.ProxyHost
{
    public class Program
    {
        /// <summary>
        /// 管理服务器地址
        /// </summary>
        public static string ManagerURL {get;set;}

        /// <summary>
        /// 
        /// </summary>
        public static string DB { get; set; }

        /// <summary>
        /// 参数很重要
        /// ags[0] 启动的URL http://IP或者域名:端口
        /// arg[1] DB地址
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (args != null && args.Length > 1)
            {
                ManagerURL = args[0];
                DB = args[1];
                return WebHost.CreateDefaultBuilder(args).UseUrls(args[0]).UseStartup<Startup>();
            }
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}
