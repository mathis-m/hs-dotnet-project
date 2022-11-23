using Common.Models;

namespace CompanySimulator.Models;

public record AccountBalance(double Balance, Currency Currency)
{
    public string FormattedValueWithIsoCode => $"{Math.Round(Balance, 2)} {Currency.IsoCode}";
    public string FormattedValueWithSymbol => $"{Math.Round(Balance, 2)} {Currency.Symbol}";
}