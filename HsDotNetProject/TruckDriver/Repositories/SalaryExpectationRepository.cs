using Microsoft.Extensions.Options;
using TruckDriver.Models;
using TruckDriver.Options;

namespace TruckDriver.Repositories;

public class SalaryExpectationRepository : ISalaryExpectationRepository
{
    private readonly Dictionary<string, Currency> _currencyCache = new();

    private readonly IOptions<SalaryExpectationLimitsConfig> _options;

    public SalaryExpectationRepository(IOptions<SalaryExpectationLimitsConfig> options)
    {
        _options = options;
    }

    private int LowerLimit => _options.Value.LowerLimit;
    private int UpperLimit => _options.Value.UpperLimit;

    private Currency Currency
    {
        get
        {
            var currentOptions     = _options.Value;
            var currentCurrencyIso = currentOptions.CurrencyIso;
            if (!_currencyCache.ContainsKey(currentCurrencyIso))
                _currencyCache.Add(currentCurrencyIso, new Currency(
                    currentOptions.CurrencyIso,
                    currentOptions.CurrencySymbol
                ));

            return _currencyCache[currentCurrencyIso];
        }
    }

    public IEnumerable<SalaryExpectation> GetAll()
    {
        return Enumerable.Range(LowerLimit, UpperLimit - LowerLimit)
            .Select(value => new SalaryExpectation(value, Currency));
    }

    public Task<IEnumerable<SalaryExpectation>> GetAllAsync()
    {
        return Task.FromResult(GetAll());
    }

    public SalaryExpectation GetById(int salaryPerMonth)
    {
        var allExpectations = GetAll();
        return GetSalaryExpectationByMonthlyValue(salaryPerMonth, allExpectations);
    }

    public async Task<SalaryExpectation> GetByIdAsync(int salaryPerMonth)
    {
        var allExpectations = await GetAllAsync();
        return GetSalaryExpectationByMonthlyValue(salaryPerMonth, allExpectations);
    }

    private SalaryExpectation GetSalaryExpectationByMonthlyValue(
        int salaryPerMonth,
        IEnumerable<SalaryExpectation> allExpectations
    )
    {
        var salaryExpectation = allExpectations.FirstOrDefault(x => x.SalaryPerMonth == salaryPerMonth);
        if (salaryExpectation == null)
            throw new InvalidOperationException(
                $"The salary expectation per month must be within ${LowerLimit} and ${UpperLimit}");

        return salaryExpectation;
    }
}