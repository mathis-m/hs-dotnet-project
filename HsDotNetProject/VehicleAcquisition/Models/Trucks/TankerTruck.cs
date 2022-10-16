namespace VehicleAcquisition.Models.Trucks;

public record TankerTruck(Size Size, int Age, Location Location) : Truck("Tanker truck", Size, Age, Location)
{
    public override int MaxPayloadInTons => Size switch
    {
        Size.Small => 2,
        Size.Medium => 4,
        Size.Large => 8,
        Size.ExtraLarge => 10,
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

    public override string ToString()
    {
        return base.ToString();
    }
}