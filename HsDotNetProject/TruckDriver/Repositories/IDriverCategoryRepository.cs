using Common.Repository;
using TruckDriver.Models;

namespace TruckDriver.Repositories;

public interface IDriverCategoryRepository : IReadonlyRepository<DriverCategory, string>
{
}