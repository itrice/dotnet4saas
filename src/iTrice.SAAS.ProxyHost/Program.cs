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
        /// 参数很重要
        /// ags[0] 启动的URL http://IP或者域名:端口
        /// arg[1] 管理服务器地址 http://IP或者域名:端口
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
                ManagerURL = args[1];
                return WebHost.CreateDefaultBuilder(args).UseUrls(args[0]).UseStartup(Assembly.GetExecutingAssembly().FullName);
            }
            return WebHost.CreateDefaultBuilder(args).UseStartup(Assembly.GetExecutingAssembly().FullName);
        }
    }
}
