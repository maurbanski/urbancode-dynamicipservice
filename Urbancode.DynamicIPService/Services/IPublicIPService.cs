
namespace Urbancode.DynamicIPService.Services
{
    public interface IPublicIPService
    {
        Task<string> GetPublicIPAddress();
    }
}