using Common.Models;

namespace FreightMarket.Models;

public record TenderCompensation(int Value, Currency Currency)
{
    public string FormattedValueWithIsoCode => $"{Value} {Currency.IsoCode}";
    public string FormattedValueWithSymbol => $"{Value} {Currency.Symbol}";
}