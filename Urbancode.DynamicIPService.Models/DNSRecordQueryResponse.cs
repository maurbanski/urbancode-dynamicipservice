using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbancode.DynamicIPService.Models
{
    public class DNSRecordQueryResponse : DNSAPIResponseBase
    {
        public IList<DNSRecord> Result { get; set; }
    }
}
