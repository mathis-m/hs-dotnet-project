using Common.Repository;
using TruckDriver.Models;

namespace TruckDriver.Repositories;

public interface INameRepository : IReadonlyRepository<Name, string>
{
}