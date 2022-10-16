using Common.Models;

namespace VehicleAcquisition.Models;

public record Price(double Value, Currency Currency)
{
    public override string ToString()
    {
        return $"{Math.Round(Value, 2)} {Currency.IsoCode}";
    }
}