using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbancode.DynamicIPService.Models
{
    public class DNSRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DNSRecordType Type { get; set; }
        public string Content { get; set; }
        public bool Proxiable { get; set; }
        public bool Proxied { get; set; }
        public int TTL {  get; set; }

    }
}
