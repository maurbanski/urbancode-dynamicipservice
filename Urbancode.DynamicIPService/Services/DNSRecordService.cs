using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Urbancode.DynamicIPService.Models;

namespace Urbancode.DynamicIPService.Service.Services
{
    public class DNSRecordService : IDNSRecordService
    {
        private readonly string _apiBaseUrl;
        private readonly string _apiZoneId;
        private readonly string _apiToken;

        private readonly HttpClient _httpClient;


        public DNSRecordService(IConfiguration configuration)
        {
            _apiBaseUrl = configuration.GetSection("DNSApiConfig:BaseUrl").Value;
            _apiZoneId = configuration.GetSection("DNSApiConfig:ZoneId").Value;
            _apiToken = configuration.GetSection("DNSApiConfig:Token").Value;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
        }

        public async Task UpdateDNSARecords(string ipAddress)
        {
            var aTypeRecords = await GetDNSRecords(DNSRecordType.A);

            foreach (var record in aTypeRecords) 
            {
                var update = new DNSRecordUpdate
                {
                    Name = record.Name,
                    Type = Enum.GetName(typeof(DNSRecordType), DNSRecordType.A),
                    Content = ipAddress,
                    Proxied = record.Proxied,
                    TTL = record.TTL
                };

                await OverWriteDNSRecord(record.Id, update);
            }
        }

        public async Task<IList<string>> GetCurrentARecordIPAddresses()
        {
            var dnsARecords = await GetDNSRecords(DNSRecordType.A);
            return dnsARecords.Select(x => x.Content).ToList();
        }

        private async Task OverWriteDNSRecord(string recordId, DNSRecordUpdate update)
        {
            var url = $"{_apiBaseUrl}/zones/{_apiZoneId}/dns_records/{recordId}";
            var requestContent = new StringContent(JsonConvert.SerializeObject(update), Encoding.UTF8, "application/json");

            var responseRaw = await _httpClient.PutAsync(url, requestContent);
            await HandleAPIResponse<DNSRecordUpdateResponse>(responseRaw);
        }

        private async Task<IList<DNSRecord>> GetDNSRecords(DNSRecordType recordType)
        {
            var type = Enum.GetName(recordType);
            var url = $"{_apiBaseUrl}/zones/{_apiZoneId}/dns_records?type={type}";

            var responseRaw = await _httpClient.GetAsync(url);
            var response = await HandleAPIResponse<DNSRecordQueryResponse>(responseRaw);
            return response.Result;
        }

        private async Task<TResponse> HandleAPIResponse<TResponse>(HttpResponseMessage httpResponseMessage) where TResponse : DNSAPIResponseBase
        {
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
            else
            {
                Exception exception;
                try
                {
                    var response = JsonConvert.DeserializeObject<TResponse>(responseContent);
                    var internalAPIExceptions = String.Join(";", response.Errors.Select(x => x.Message));
                    exception = new Exception($"API response: {httpResponseMessage.StatusCode}, internal API exceptions: [{internalAPIExceptions}]");
                }
                catch (Exception)
                {
                    exception = new Exception($"API response: {httpResponseMessage.StatusCode}");
                }

                throw exception;
            }
        }
    }
}
