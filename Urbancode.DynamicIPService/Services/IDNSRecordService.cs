
namespace Urbancode.DynamicIPService.Service.Services
{
    public interface IDNSRecordService
    {
        Task UpdateDNSARecords(string ipAddress);
        Task<IList<string>> GetCurrentARecordIPAddresses();
    }
}