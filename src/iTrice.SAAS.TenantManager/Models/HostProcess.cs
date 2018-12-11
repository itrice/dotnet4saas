using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iTrice.SAAS.TenantManager.Models
{
    public class HostProcess
    {
        public Guid TenantId { get; set; }

        public string TenantName { get; set; }

        public string ProcessId { get; set; }

        public string ProcessName { get; set; }

        public string CPU { get; set; }

        public string Memory { get; set; }

        public string URL { get; set; }
    }
}
