namespace Common.BngUtils;

public interface IBngDistanceCalculator
{
    double CalculateDistanceInMeters(BngPoint start, BngPoint end);
}