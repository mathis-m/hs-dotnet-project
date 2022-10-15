namespace TruckDriver.Models;

public record SalaryExpectation(int SalaryPerMonth, Currency Currency)
{
    public string FormattedValueWithIsoCode => $"{SalaryPerMonth} {Currency.IsoCode}";
    public string FormattedValueWithSymbol => $"{SalaryPerMonth} {Currency.Symbol}";
}