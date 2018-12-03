using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using iTrice.SAAS.TenantManager.Data;
using Microsoft.AspNetCore.Mvc;
using iTrice.SAAS.TenantManager.Models;
using iTrice.SAAS.TenantManager.Server;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace iTrice.SAAS.TenantManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly TenantContext _context;

        public HomeController(TenantContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
