using System.Threading.Tasks;

namespace AddressValidator.Domain
{
    public interface IJob
    {
        Task RunTask();
    }
}