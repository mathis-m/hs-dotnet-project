using TruckDriver.Models;

namespace TruckDriver.Services;

public class TruckOperatorGeneratorService : IGeneratorService<TruckOperator>
{
    private readonly IGeneratorService<DriverCategory> _driverCategoryGenerator;
    private readonly IGeneratorService<Name> _nameGenerator;
    private readonly IGeneratorService<SalaryExpectation> _salaryExpectationGenerator;

    public TruckOperatorGeneratorService(IGeneratorService<Name> nameGenerator,
        IGeneratorService<SalaryExpectation> salaryExpectationGenerator,
        IGeneratorService<DriverCategory> driverCategoryGenerator)
    {
        _nameGenerator = nameGenerator;
        _salaryExpectationGenerator = salaryExpectationGenerator;
        _driverCategoryGenerator = driverCategoryGenerator;
    }

    public async Task<List<TruckOperator>> GenerateAsync(int count)
    {
        var names = await _nameGenerator.GenerateAsync(count);
        var salaryExpectations = await _salaryExpectationGenerator.GenerateAsync(count);
        var categories = await _driverCategoryGenerator.GenerateAsync(count);

        var drivers = new List<TruckOperator>();
        for (var idx = 0; idx < count; idx++)
            drivers.Add(new TruckOperator(names[idx], salaryExpectations[idx], categories[idx]));

        return drivers;
    }
}