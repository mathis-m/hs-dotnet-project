using Common.BngUtils;
using CompanySimulator.Models;
using TruckDriver.Models;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Factories;

public class RelocationStatsFactory : IRelocationStatsFactory
{
    private const double BaseAvgKmH = 70;

    private const double DreamySpeedFixInKmH = -2;
    private const double RacerSpeedFixInKmH  = 3;

    private const double RacerConsumptionFixInPercent = 1.025;

    private const double PowerNeededPerTon = 7.5;

    private const double WorkingHoursPerDay = 8;


    private static Dictionary<string, BngPoint> CityLocationLookup = new()
    {
        { "Amsterdam", new BngPoint(868851, 297477) },
        { "Berlin", new BngPoint(1442341, 404144) },
        { "Esslingen", new BngPoint(1232391, -71899) },
        { "Rom", new BngPoint(1605258, -786717) },
        { "Lissabon", new BngPoint(-220417, -1218006) },
        { "Istanbul", new BngPoint(3015490, -498084) },
        { "Aarhus", new BngPoint(1156381, 763352) },
        { "Tallinn", new BngPoint(1889074, 1368933) },
    };

    private readonly IBngDistanceCalculator _bngDistanceCalculator;

    public RelocationStatsFactory(IBngDistanceCalculator bngDistanceCalculator)
    {
        _bngDistanceCalculator = bngDistanceCalculator;
    }


    public RelocationStats Create(Truck truck, TruckOperator driver, Location destination, double payloadInT)
    {
        var avgSpeed    = GetAvgSpeed(truck, driver, payloadInT);
        var consumption = GetConsumption(truck, driver);
        var distance    = GetDistanceInKm(truck.Location, destination);
        var duration    = GetDurationInDays(avgSpeed, distance);

        return new RelocationStats(avgSpeed, consumption, distance, duration);
    }


    public DateTime CalculateArrival(RelocationStats stats, DateTime start)
    {
        return start.Add(stats.DurationInDays);
    }

    private static TimeSpan GetDurationInDays(double avgSpeed, double distance)
    {
        var exactHours = distance / avgSpeed;
        var exactDays = exactHours / WorkingHoursPerDay;

        return TimeSpan.FromDays(exactDays);
    }

    private double GetDistanceInKm(Location start, Location destination)
    {
        var startBng = CityLocationLookup[start.City];
        var endBng   = CityLocationLookup[destination.City];

        var meters = _bngDistanceCalculator.CalculateDistanceInMeters(startBng, endBng);

        return meters / 1000;
    }

    private static double GetConsumption(ITruck truck, TruckOperator driver)
    {
        var consumption = driver.DriverCategory.Type switch
        {
            DriverCategory.RacerType => truck.ConsumptionPer100KmInL * RacerConsumptionFixInPercent,
            _ => truck.ConsumptionPer100KmInL,
        };
        return consumption;
    }

    private static double GetAvgSpeed(ITruck truck, TruckOperator driver, double payloadInT)
    {
        var avgSpeed = driver.DriverCategory.Type switch
        {
            DriverCategory.DreamyType => BaseAvgKmH + DreamySpeedFixInKmH,
            DriverCategory.RacerType => BaseAvgKmH + RacerSpeedFixInKmH,
            _ => BaseAvgKmH,
        };

        var powerNeededForPayload = payloadInT * PowerNeededPerTon;
        if (truck.EnginePowerInKw < powerNeededForPayload)
        {
            var ratio = truck.EnginePowerInKw / powerNeededForPayload;
            avgSpeed *= ratio;
        }

        return avgSpeed;
    }
}