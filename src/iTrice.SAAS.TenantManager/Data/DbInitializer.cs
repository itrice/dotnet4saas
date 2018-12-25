using iTrice.SAAS.TenantManager.Data;
using iTrice.SAAS.TenantManager.Models;
using System;
using System.Linq;

namespace iTrice.SAAS.TenantManager.Server
{
    public static class DbInitializer
    {
        public static void Initialize(TenantContext context)
        {
            context.Database.EnsureCreated();

            // Look for any Tenants.
            if (context.Tenants.Any())
            {
                return;   // DB has been seeded
            }

            // 添加默认数据
            var ts = new Tenant[]
            {
                new Tenant{Id = Guid.NewGuid(),Name = "admin",Password = "123",CreateTime = DateTime.Now},
            };

            foreach (var t in ts)
            {
                context.Tenants.Add(t);
            }
            context.SaveChanges();
        }
    }
}