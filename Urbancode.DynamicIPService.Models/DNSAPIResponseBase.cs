using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbancode.DynamicIPService.Models
{
    public abstract class DNSAPIResponseBase
    {
        public bool Success { get; set; }
        public IList<CloudflareAPIResponseMessage> Errors { get; set; }
        public IList<CloudflareAPIResponseMessage> Messages { get; set; }
    }
}
