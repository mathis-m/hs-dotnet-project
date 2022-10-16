namespace VehicleAcquisition.Models.Trucks;

public record FlatbedTruck(Size Size, int Age, Location Location) : Truck("Flatbed truck", Size, Age, Location)
{
    public override int MaxPayloadInTons => Size switch
    {
        Size.Small => 4,
        Size.Medium => 6,
        Size.Large => 7,
        Size.ExtraLarge => 10,
        _ => throw new ArgumentOutOfRangeException(nameof(Size)),
    };

    protected override int BaseConsumptionPer100KmInL => Size switch
    {
        Size.Small => 10,
        Size.Medium => 12,
        Size.Large => 16,
        Size.ExtraLarge => 22,
        _ => throw new ArgumentOutOfRangeException(nameof(Size)),
    };

    public override string ToString()
    {
        return base.ToString();
    }
}