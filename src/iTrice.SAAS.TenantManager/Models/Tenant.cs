using System;

namespace iTrice.SAAS.TenantManager.Models
{
    public class Tenant
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// 域名地址
        /// </summary>
        public string URL { get; set; }

        public int Flag { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public string DataBase { get; set; }
    }
}