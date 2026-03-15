using NLog;
using NLog.Extensions.Logging;
using Urbancode.DynamicIPService;
using Urbancode.DynamicIPService.Service.Services;
using Urbancode.DynamicIPService.Services;

var builder = Host.CreateApplicationBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromSection(builder.Configuration).GetCurrentClassLogger();
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
builder.Logging.AddNLog();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

try
{
    builder.Services.AddMemoryCache();
    builder.Services.AddTransient<IPublicIPService, PublicIPService>();
    builder.Services.AddTransient<IDNSRecordService, DNSRecordService>();

    builder.Services.AddHostedService<Worker>();

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    logger.Log(NLog.LogLevel.Fatal, ex);
    throw;
}


