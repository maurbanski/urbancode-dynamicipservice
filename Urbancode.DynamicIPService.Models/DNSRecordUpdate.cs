using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbancode.DynamicIPService.Models
{
    public class DNSRecordUpdate
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public bool Proxied { get; set; }
        public int TTL {  get; set; }
    }
}
