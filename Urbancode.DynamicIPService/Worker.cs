using Urbancode.DynamicIPService.Service.Services;
using Urbancode.DynamicIPService.Services;

namespace Urbancode.DynamicIPService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPublicIPService _publicIPService;
        private readonly IDNSRecordService _dnsRecordService;
        private readonly int _pollingIntervalSeconds;

        public Worker(ILogger<Worker> logger, IPublicIPService publicIPService, IDNSRecordService dnsRecordService, IConfiguration configuration)
        {
            _logger = logger;
            _publicIPService = publicIPService;
            _dnsRecordService = dnsRecordService;
            _pollingIntervalSeconds = Int32.Parse(configuration.GetSection("Variables:PollingIntervalSeconds").Value);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var aRecordIPAddresses = await _dnsRecordService.GetCurrentARecordIPAddresses();

            if (!stoppingToken.IsCancellationRequested) _logger.LogInformation("Process starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var currentPublicIP = await _publicIPService.GetPublicIPAddress();
                    if (aRecordIPAddresses.Where(x => x != currentPublicIP).Any())
                    {
                        await _dnsRecordService.UpdateDNSARecords(currentPublicIP);
                        aRecordIPAddresses = await _dnsRecordService.GetCurrentARecordIPAddresses();
                        _logger.LogInformation($"Updated DNS A records with {currentPublicIP}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(_pollingIntervalSeconds), stoppingToken);
            }

            _logger.LogInformation("Process stopping");
        }
    }
}
