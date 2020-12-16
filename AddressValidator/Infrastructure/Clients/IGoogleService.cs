using System.Threading.Tasks;
using AddressValidator.Infrastructure.Models;

namespace AddressValidator.Infrastructure.Clients
{
    public interface IGoogleService
    {
        Task<GoogleResponse> ValidateAsync(string address);
    }
}
