using Common.Models;

namespace FreightMarket.Models;

public record TenderPenalty(double Value, Currency Currency)
{
    public string FormattedValueWithIsoCode => $"{Math.Round(Value, 2)} {Currency.IsoCode}";
    public string FormattedValueWithSymbol => $"{Math.Round(Value, 2)} {Currency.Symbol}";
}