using Microsoft.Extensions.Options;
using TruckDriver.Models;
using TruckDriver.Options;

namespace TruckDriver.Services;

public class SalaryExpectationGeneratorService : IGeneratorService<SalaryExpectation>
{
    private readonly SalaryExpectationLimitsConfig _config;
    private readonly Random _random = new();

    public SalaryExpectationGeneratorService(IOptions<SalaryExpectationLimitsConfig> options)
    {
        _config = options.Value;
    }

    private Currency CurrencyFromConfig => new(_config.CurrencyIso, _config.CurrencySymbol);

    public Task<List<SalaryExpectation>> GenerateAsync(int count)
    {
        var expectations = new List<SalaryExpectation>();
        for (var i = 0; i < count; i++)
        {
            var salaryExpectation = GetRandomSalaryExpectation();
            expectations.Add(salaryExpectation);
        }

        return Task.FromResult(expectations);
    }

    private SalaryExpectation GetRandomSalaryExpectation()
    {
        var salary = _random.Next(_config.LowerLimit, _config.UpperLimit + 1);
        var salaryExpectation = new SalaryExpectation(salary, CurrencyFromConfig);
        return salaryExpectation;
    }
}