namespace VehicleAcquisition.Models;

public readonly record struct Location(string City)
{
    public override string ToString()
    {
        return City;
    }
}