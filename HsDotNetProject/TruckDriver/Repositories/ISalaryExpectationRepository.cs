using Common.Repository;
using TruckDriver.Models;

namespace TruckDriver.Repositories;

public interface ISalaryExpectationRepository : IReadonlyRepository<SalaryExpectation, int>
{
}