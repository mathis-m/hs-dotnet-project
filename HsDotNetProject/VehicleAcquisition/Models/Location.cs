namespace VehicleAcquisition.Models;

public record Location(string City)
{
    public override string ToString()
    {
        return City;
    }
}