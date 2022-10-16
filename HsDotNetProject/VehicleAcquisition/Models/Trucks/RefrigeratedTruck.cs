namespace VehicleAcquisition.Models.Trucks;

public record RefrigeratedTruck(Size Size, int Age, Location Location) : Truck("Refrigerated truck", Size, Age, Location)
{
    public override int MaxPayloadInTons => Size switch
    {
        Size.Small => 3,
        Size.Medium => 4,
        Size.Large => 5,
        Size.ExtraLarge => 6,
        _ => throw new ArgumentOutOfRangeException(nameof(Size)),
    };

    protected override int BaseConsumptionPer100KmInL => Size switch
    {
        Size.Small => 14,
        Size.Medium => 18,
        Size.Large => 20,
        Size.ExtraLarge => 30,
        _ => throw new ArgumentOutOfRangeException(nameof(Size)),
    };

    protected override double CalculatePriceFactor()
    {
        var factor = base.CalculatePriceFactor();
        return factor + .1;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}