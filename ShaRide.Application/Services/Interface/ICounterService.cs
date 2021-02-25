using System.Threading.Tasks;

namespace ShaRide.Application.Services.Interface
{
    public interface ICounterService
    {
        Task<int> GetNextCounter(string counterName);
    }
}