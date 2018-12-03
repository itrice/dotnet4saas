using System;
using iTrice.SAAS.TenantManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using iTrice.SAAS.TenantManager.Data;
using System.Linq;
using System.Diagnostics;

namespace iTrice.SAAS.TenantManager.Controllers
{
    public class TenantController : Controller
    {
        private readonly TenantContext _context;

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

        public JsonResult RegistTeant(string name)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}