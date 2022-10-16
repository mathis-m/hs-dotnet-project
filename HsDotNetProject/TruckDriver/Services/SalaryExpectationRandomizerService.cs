using TruckDriver.Models;
using TruckDriver.Repositories;

namespace TruckDriver.Services;

public class SalaryExpectationRandomizerService : BaseRandomizerService<SalaryExpectation>
{
    private readonly ISalaryExpectationRepository _repository;

    public SalaryExpectationRandomizerService(ISalaryExpectationRepository repository)
    {
        _repository = repository;
    }

    public override async Task<SalaryExpectation> NextAsync()
    {
        var allSalaryExpectations = await _repository.GetAllAsync();
        return GetRandomItem(allSalaryExpectations);
    }
}