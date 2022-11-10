using VehicleAcquisition.Factories;

namespace VehicleAcquisition.Models.Trucks;

public record FlatbedTruck(Size Size, Age Age, Location Location) : Truck(TruckTypes.FlatbedTruck, Size, Age, Location)
{
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