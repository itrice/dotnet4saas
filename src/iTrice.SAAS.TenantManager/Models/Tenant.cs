using System;

namespace iTrice.SAAS.TenantManager.Models
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Flag { get; set; }
        public DateTime CreateTime { get; set; }
    }
}