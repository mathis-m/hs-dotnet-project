using Common.Models;

namespace FreightMarket.Models;

public record TenderCompensation(double Value, Currency Currency)
{
    public string FormattedValueWithIsoCode => $"{Math.Round(Value, 2)} {Currency.IsoCode}";
    public string FormattedValueWithSymbol => $"{Math.Round(Value, 2)} {Currency.Symbol}";
}