using VehicleAcquisition.Factories;

namespace VehicleAcquisition.Models.Trucks;

public record RefrigeratedTruck(Size Size, Age Age, Location Location) : Truck(TruckTypes.RefrigeratedTruck, Size, Age, Location)
{
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