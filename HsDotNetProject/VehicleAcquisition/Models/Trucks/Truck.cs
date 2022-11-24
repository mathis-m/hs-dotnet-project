using Common.Models;
using VehicleAcquisition.Factories;

namespace VehicleAcquisition.Models.Trucks;

public abstract record Truck(TruckTypes TruckType, Size Size, Age Age, Location Location) : ITruck
{
    private static readonly Currency EurCurrency = new("EUR", "€");
    private static readonly Random   Random      = new();

    private static readonly Dictionary<Size, EnginePowerBounds> PowerBoundsPerSize = new()
    {
        { Size.Small, new EnginePowerBounds(10, 25) },
        { Size.Medium, new EnginePowerBounds(30, 50) },
        { Size.Large, new EnginePowerBounds(40, 70) },
        { Size.ExtraLarge, new EnginePowerBounds(60, 80) },
    };

    private static readonly Dictionary<Size, Price> BasePricePerSize = new()
    {
        { Size.Small, new Price(25000, EurCurrency) },
        { Size.Medium, new Price(65000, EurCurrency) },
        { Size.Large, new Price(80000, EurCurrency) },
        { Size.ExtraLarge, new Price(120000, EurCurrency) },
    };

    private int? _randomEnginePowerInKw;

    private int ConsumptionCorrectionForAge => Age.AgeInYears / 3;

    public string FormattedAge => Age.AgeInYears switch
    {
        0 => "-new-",
        1 => "1 year",
        _ => $"{Age} years",
    };

    protected abstract int BaseConsumptionPer100KmInL { get; }

    public int EnginePowerInKw
    {
        get
        {
            if (_randomEnginePowerInKw != null) return _randomEnginePowerInKw.Value;

            var powerBounds           = PowerBoundsPerSize[Size];
            var randomEnginePowerInKw = Random.Next(powerBounds.LowerLimit, powerBounds.UpperLimit + 1);
            _randomEnginePowerInKw = randomEnginePowerInKw;
            return randomEnginePowerInKw;
        }
    }

    public int ConsumptionPer100KmInL => BaseConsumptionPer100KmInL + ConsumptionCorrectionForAge;
    public Price Price => CalculatePrice();
    public int MaxPayloadInTons => MaxPayloadFactory.GetMaxPayloadFor(TruckType, Size);

    public override string ToString()
    {
        return $"{TruckType}, {FormattedAge}, {EnginePowerInKw} kW, {MaxPayloadInTons} T, {ConsumptionPer100KmInL} l, {Location}, {Price}";
    }

    private Price CalculatePrice()
    {
        var priceFactor = CalculatePriceFactor();

        var basePrice       = BasePricePerSize[Size];
        var calculatedPrice = priceFactor * basePrice.Value;

        return basePrice with
        {
            Value = calculatedPrice,
        };
    }

    protected virtual double CalculatePriceFactor()
    {
        var randomFactor = Random.Next(-20, 31);
        var priceFactor  = 1 + randomFactor / 100.0 - Age.AgeInYears * 0.03;
        return priceFactor;
    }
}