using TruckDriver.Models;
using TruckDriver.Repositories;

namespace TruckDriver.Services;

public class SalaryExpectationGeneratorService : RandomGeneratorService<SalaryExpectation>
{
    private readonly ISalaryExpectationRepository _repository;

    public SalaryExpectationGeneratorService(ISalaryExpectationRepository repository)
    {
        _repository = repository;
    }

    public override async Task<SalaryExpectation> GenerateAsync()
    {
        var allSalaryExpectations = await _repository.GetAllAsync();
        return GetRandomItem(allSalaryExpectations);
    }
}