namespace Common.BngUtils;

public class BngDistanceCalculator : IBngDistanceCalculator
{
    public double CalculateDistanceInMeters(BngPoint start, BngPoint end)
    {
        var eastingDiffSquared = Math.Pow(start.Easting - end.Easting, 2);
        var northingDiffSquared = Math.Pow(start.Northing - end.Northing, 2);

        return Math.Sqrt(eastingDiffSquared + northingDiffSquared);
    }
}