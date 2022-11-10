using VehicleAcquisition.Factories;

namespace VehicleAcquisition.Models.Trucks;

public record TankerTruck(Size Size, Age Age, Location Location) : Truck(TruckTypes.TankerTruck, Size, Age, Location)
{
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