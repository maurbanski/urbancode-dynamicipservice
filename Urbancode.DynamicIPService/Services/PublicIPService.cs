using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Urbancode.DynamicIPService.Services
{
    public class PublicIPService : IPublicIPService
    {
        private readonly HttpClient _httpClient;
        private readonly string _ipCheckServiceURL;
        public PublicIPService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _ipCheckServiceURL = configuration.GetSection("Routing:IPCheckServiceURL").Value;
        }

        public async Task<string> GetPublicIPAddress()
        {
            var publicIP = await _httpClient.GetStringAsync(_ipCheckServiceURL);
            return publicIP;
        }
    }
}
