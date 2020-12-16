using AddressValidator.Infrastructure.Models;

namespace AddressValidator.Infrastructure
{
    public interface IAddressBuilder
    {
        string BuildAddressString(GoogleResponse googleResponse);
    }
}
