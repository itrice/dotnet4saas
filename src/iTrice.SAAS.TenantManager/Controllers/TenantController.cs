using iTrice.SAAS.TenantManager.Data;
using iTrice.SAAS.TenantManager.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace iTrice.SAAS.TenantManager.Controllers
{
    public class TenantController : Controller
    {
        private static object _lock = new object();

        private readonly TenantContext _context;

        private Dictionary<Guid, Process> _hosts = new Dictionary<Guid, Process>();

        public TenantController(TenantContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ts = _context.Tenants.Where(o => o.Flag == 0).ToList();
            ViewBag.tenants = ts;

            ViewData["Message"] = "Your application description page.";

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

        public JsonResult RegistTeant(string name,string url)
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
                var tenant = new Tenant();
                tenant.Id = Guid.NewGuid();
                tenant.Name = name;
                tenant.CreateTime = DateTime.Now;
                tenant.URL = url;
                tenant.Flag = 0;
                _context.Tenants.Add(tenant);
                _context.SaveChanges();
                rst.Code = 1;
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
        /// start the tenant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult StartTenantServer(Guid id)
        {
            var rs = new ResultMessage();
            if (!_hosts.ContainsKey(id))
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
                    var filePath = @"E:\Source\github\personal\src\iTrice.SAAS.BusinessSystem\bin\Release\netcoreapp2.1\win-x64\iTrice.SAAS.BusinessSystem.exe";
                    var startInfo = new ProcessStartInfo();
                    startInfo.FileName = filePath;
                    startInfo.Arguments = $"{tenant.URL} {tenant.Name}";
                    startInfo.WorkingDirectory = @"E:\Source\github\personal\src\iTrice.SAAS.BusinessSystem\bin\Release\netcoreapp2.1\win-x64\";
                    Console.WriteLine($"{filePath}{tenant.URL}");
                    Process pro = System.Diagnostics.Process.Start(startInfo);

                    lock (_lock)
                    {
                        _hosts.Add(id, pro);
                    }
                    pro.WaitForExit();
                    lock (_lock)
                    {
                        _hosts.Remove(id);
                    }
                });
            }


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
            if (!_hosts.ContainsKey(id))
            {
                var pro = _hosts[id];
                pro.Close();
                var tenant = _context.Tenants.FirstOrDefault(o => o.Id == id);
                rs.Code = 1;
                rs.Message = $"已经停用租户:{tenant.Name}";
                return Json(rs);
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
                if (!_hosts.ContainsKey(id))
                {
                    rs.Code = -1;
                    return Json(rs);
                }
                var host = _hosts[id];
                rs.Code = 1;
                rs.Data = new
                {
                    Name = host.ProcessName,
                    CPU = host.UserProcessorTime.TotalMilliseconds / host.TotalProcessorTime.TotalMilliseconds,
                    MEMORY = host.VirtualMemorySize,
                    NETWORK = ""
                };
            }

            return Json(rs);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}