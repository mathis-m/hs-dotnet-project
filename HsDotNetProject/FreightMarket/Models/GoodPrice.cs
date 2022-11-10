using Common.Models;

namespace FreightMarket.Models;

public record GoodPrice(int MinPricePerTon, Currency Currency)
{
    public string FormattedValueWithIsoCode => $"{MinPricePerTon} {Currency.IsoCode}";
    public string FormattedValueWithSymbol => $"{MinPricePerTon} {Currency.Symbol}";
}