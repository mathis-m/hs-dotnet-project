using CompanySimulator.Models;
using TruckDriver.Models;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Factories;

public interface IRelocationStatsFactory
{
    RelocationStats Create(Truck truck, TruckOperator driver, Location destination, double payloadInT);
    DateTime CalculateArrival(RelocationStats stats, DateTime start);
}