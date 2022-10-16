using TruckDriver.Models;

namespace TruckDriver.Services;

public class TruckOperatorGeneratorService : IGeneratorService<TruckOperator>
{
    private readonly IGeneratorService<DriverCategory>    _driverCategoryGenerator;
    private readonly IGeneratorService<Name>              _nameGenerator;
    private readonly IGeneratorService<SalaryExpectation> _salaryExpectationGenerator;

    public TruckOperatorGeneratorService(IGeneratorService<Name> nameGenerator,
        IGeneratorService<SalaryExpectation> salaryExpectationGenerator,
        IGeneratorService<DriverCategory> driverCategoryGenerator)
    {
        _nameGenerator              = nameGenerator;
        _salaryExpectationGenerator = salaryExpectationGenerator;
        _driverCategoryGenerator    = driverCategoryGenerator;
    }

    public async Task<TruckOperator> GenerateAsync()
    {
        var name              = await _nameGenerator.GenerateAsync();
        var category          = await _driverCategoryGenerator.GenerateAsync();
        var salaryExpectation = await _salaryExpectationGenerator.GenerateAsync();

        return new TruckOperator(name, salaryExpectation, category);
    }
}