using Common.Services;
using TruckDriver.Models;

namespace TruckDriver.Services;

public class TruckOperatorRandomizerService : IRandomizerService<TruckOperator>
{
    private readonly IRandomizerService<DriverCategory>    _driverCategoryRandomizer;
    private readonly IRandomizerService<Name>              _nameRandomizer;
    private readonly IRandomizerService<SalaryExpectation> _salaryExpectationRandomizer;

    public TruckOperatorRandomizerService(IRandomizerService<Name> nameRandomizer,
        IRandomizerService<SalaryExpectation> salaryExpectationRandomizer,
        IRandomizerService<DriverCategory> driverCategoryRandomizer)
    {
        _nameRandomizer              = nameRandomizer;
        _salaryExpectationRandomizer = salaryExpectationRandomizer;
        _driverCategoryRandomizer    = driverCategoryRandomizer;
    }

    public async Task<TruckOperator> NextAsync()
    {
        var name              = await _nameRandomizer.NextAsync();
        var category          = await _driverCategoryRandomizer.NextAsync();
        var salaryExpectation = await _salaryExpectationRandomizer.NextAsync();

        return new TruckOperator(name, salaryExpectation, category);
    }
}