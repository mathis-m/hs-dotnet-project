using TruckDriver.Models;

namespace TruckDriver.Providers;

public interface INameProvider
{
    Task<IEnumerable<Name>> GetAllAsync();
    IEnumerable<Name> GetAll();
}