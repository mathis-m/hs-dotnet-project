namespace VehicleAcquisition.Models.Trucks;

public interface ITruck
{
    string TruckType { get; }
    int EnginePowerInKw { get; }
    int ConsumptionPer100KmInL { get; }
    Price Price { get; }
    Size Size { get; init; }
    int Age { get; init; }
    Location Location { get; init; }
    int MaxPayloadInTons { get; }
    string ToString();
}