namespace VehicleAcquisition.Models;

public record Age(int AgeInYears)
{
    public override string ToString()
    {
        return $"{AgeInYears}";
    }
}