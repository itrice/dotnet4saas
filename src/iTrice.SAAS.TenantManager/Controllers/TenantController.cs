using iTrice.SAAS.TenantManager.Data;
using iTrice.SAAS.TenantManager.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace iTrice.SAAS.TenantManager.Controllers
{
    public class TenantController : Controller
    {
        private static object _lock = new object();

        private readonly TenantContext _context;

        public TenantController(TenantContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        static TenantController()
        {
            HostInstance = new Dictionary<Guid, Process>();
        }

        public static Dictionary<Guid, Process> HostInstance { get; private set; }

        private IConfiguration Configuration { get; }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 宿主进程
        /// </summary>
        /// <returns></returns>
        public IActionResult Process()
        {
            var hps = _context.Tenants.Where(o => o.Flag == 0).Select(o => new HostProcess
            {
                TenantId = o.Id,
                TenantName = o.Name,
                URL = o.URL
            }).ToList();

            ViewBag.hosts = hps;

            return View();
        }

        #region 租户信息管理

        /// <summary>
        /// 注册租户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public JsonResult RegistTeant(string name, string url)
        {
            var rst = new ResultMessage();
            try
            {
                if (_context.Tenants.Any(o => o.Name == name))
                {
                    rst.Message = $"已经存在用户{name}";
                    rst.Code = -2;
                    return Json(rst);
                }
                Migrations.DBMigration.CreateDB(Configuration, "DB_" + name);
                var tenant = new Tenant();
                tenant.Id = Guid.NewGuid();
                tenant.Name = name;
                tenant.CreateTime = DateTime.Now;
                tenant.URL = url;
                tenant.Flag = 0;
                _context.Tenants.Add(tenant);
                _context.SaveChanges();
                rst.Code = 1;
                rst.Message = "注册成功";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                rst.Code = -1;
                rst.Message = e.Message;
            }

            return Json(rst);
        }

        /// <summary>
        /// 获取租户数量
        /// </summary>
        /// <param name="type">0待审核 1已审核 2到期 3注销</param>
        /// <returns></returns>
        public JsonResult GetTenantCount(int type)
        {
            var rs = new ResultMessage();
            try
            {
                rs.Data = _context.Tenants.Count(o => o.Flag == type);
                rs.Code = 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                rs.Code = -1;
                rs.Message = e.Message;
            }

            return Json(rs);
        }

        /// <summary>
        /// 获取租户
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllTenants(int page, int limit)
        {
            var rs = new ResultMessage();
            try
            {
                var totalCount = _context.Tenants.Count();
                var list = _context.Tenants.Take(page * limit).Skip((page - 1) * limit).ToList();
                rs.Data = list;
                rs.Total = totalCount;
                rs.Code = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                rs.Code = -1;
                rs.Message = e.Message;
            }

            return Json(rs);
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DelTenant(Guid id)
        {
            var rs = new ResultMessage();
            var tent = _context.Tenants.FirstOrDefault(o => o.Id == id);
            if (tent != null)
            {
                _context.Remove(tent);
            }

            rs.Code = 0;
            rs.Message = "删除成功";
            return Json(rs);
        }

        #endregion

        #region 租户状态管理

        /// <summary>
        /// start the tenant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult StartTenantServer(Guid id)
        {
            var rs = new ResultMessage();
            if (!HostInstance.ContainsKey(id))
            {
                var tenant = _context.Tenants.FirstOrDefault(o => o.Id == id);
                if (tenant == null)
                {
                    rs.Code = -1;
                    rs.Message = "未找到租户信息";

                    return Json(rs);
                }
                if (tenant.Flag == 1)
                {
                    rs.Code = -2;
                    rs.Message = "租户已停用，无法启动";

                    return Json(rs);
                }

                Task.Factory.StartNew(() =>
                {
                    var proxyHostPath = Configuration["ProxyHost"];
                    var startInfo = new ProcessStartInfo();
                    startInfo.FileName = proxyHostPath + "\\iTrice.SAAS.ProxyHost.exe";
                    startInfo.Arguments = $"{tenant.URL} DB_{tenant.Name}";
                    startInfo.WorkingDirectory = proxyHostPath;
                    Console.WriteLine($"{proxyHostPath}{tenant.URL}");

                    Process pro = System.Diagnostics.Process.Start(startInfo);


                    lock (_lock)
                    {
                        HostInstance.Add(id, pro);
                    }
                    pro.WaitForExit();
                    lock (_lock)
                    {
                        HostInstance.Remove(id);
                    }
                });
                rs.Message = "启动租户进程";
            }
            else
            {
                rs.Message = "租户已经启动";
            }

            rs.Code = 1;
            return Json(rs);
        }

        private void Pro_Exited(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Pro_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// start the tenant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult StopTenantServer(Guid id)
        {
            var rs = new ResultMessage();
            if (HostInstance.ContainsKey(id))
            {
                lock (_lock)
                {
                    var pro = HostInstance[id];
                    pro.Kill();
                }
                var tenant = _context.Tenants.FirstOrDefault(o => o.Id == id);
                rs.Code = 1;
                rs.Message = $"停用租户:{tenant.Name}";
                return Json(rs);
            }
            else
            {
                rs.Code = -1;
                rs.Message = "停止失败";
            }
            return Json(rs);
        }

        /// <summary>
        /// 获取宿主进程状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetProcessStatus(Guid id)
        {
            var rs = new ResultMessage();

            lock (_lock)
            {
                if (!HostInstance.ContainsKey(id))
                {
                    rs.Code = -1;
                    return Json(rs);
                }
                var host = HostInstance[id];
                if (!host.HasExited)
                {
                    rs.Code = 1;

                    //间隔时间内的CPU运行时间除以逻辑CPU数量
                    var value = (host.PrivilegedProcessorTime / host.TotalProcessorTime) / Environment.ProcessorCount * 100;
                    //prevCpuTime = curTime;
                    rs.Data = new
                    {
                        PID = host.Id,
                        CPU = value,
                        MEMORY = host.VirtualMemorySize,
                        RunningTime = (DateTime.Now - host.StartTime).TotalSeconds,
                        NETWORK = ""
                    };
                }
                else
                {
                    rs.Code = -2;
                }
            }

            return Json(rs);
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}