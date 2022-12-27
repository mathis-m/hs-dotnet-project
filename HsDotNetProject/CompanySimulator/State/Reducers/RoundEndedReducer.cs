using CompanySimulator.Models;
using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Reducers;

public class RoundEndedReducer : IReducer
{
    private const double FuelPricePerLiter = 1;

    public RootState Reduce(RootState currentState)
    {
        var currentDate = currentState.SimulationState.SimulationDate;
        var nextDate    = currentDate.Add(TimeSpan.FromDays(1));

        var (derivedRequests, derivedTrucks, derivedAssignments) = DeriveArrivedTrucksAndDependencies(currentState.CompanyState, nextDate, out var sumFuelCost);

        var derivedAccountBalance = currentState.CompanyState.AccountBalance with
        {
            Balance = currentState.CompanyState.AccountBalance.Balance - sumFuelCost,
        };

        var derivedCompanyState = currentState.CompanyState with
        {
            TruckRelocationRequests = derivedRequests,
            OwnedTrucks = derivedTrucks,
            TruckAssignments = derivedAssignments,
            AccountBalance = derivedAccountBalance,
        };

        var derivedSimulationState = currentState.SimulationState with
        {
            SimulationDate = nextDate,
        };

        return currentState with
        {
            SimulationState = derivedSimulationState,
            CompanyState = derivedCompanyState,
        };
    }

    private static (Dictionary<Truck, RelocationRequest>, IReadOnlyList<Truck>, Dictionary<TruckOperator, Truck>) DeriveArrivedTrucksAndDependencies(CompanyState state,
        DateTime nextDate,
        out double sumFuelCost)
    {
        var derivedRequests    = new Dictionary<Truck, RelocationRequest>();
        var derivedAssignments = new Dictionary<TruckOperator, Truck>(state.TruckAssignments);
        var derivedTrucks      = new List<Truck>(state.OwnedTrucks);

        sumFuelCost = 0.0;
        foreach (var (truck, relocation) in state.TruckRelocationRequests)
        {
            if (relocation.Status != RelocationStatus.RelocationStarted)
            {
                derivedRequests[truck] = relocation;
                continue;
            }

            ;
            if (relocation.Stats == null)
            {
                derivedRequests[truck] = relocation;
                continue;
            }

            ;
            if (!relocation.EstimatedArrival.HasValue)
            {
                derivedRequests[truck] = relocation;
                continue;
            }

            ;
            if (relocation.EstimatedArrival.Value.Date > nextDate.Date)
            {
                derivedRequests[truck] = relocation;
                continue;
            }

            ;

            var derivedTruck = truck with
            {
                Location = relocation.TargetLocation,
            };

            UpdateRelocationStatus(derivedRequests, derivedTruck, relocation);
            UpdateAssignments(derivedAssignments, truck, derivedTruck);
            UpdateTrucks(derivedTrucks, truck, derivedTruck);

            sumFuelCost += CalculateFuelCost(relocation.Stats.Distance, relocation.Stats.ConsumptionPer100Km);
        }

        return (derivedRequests, derivedTrucks, derivedAssignments);
    }

    private static void UpdateTrucks(IList<Truck> derivedTrucks, Truck truck, Truck derivedTruck)
    {
        var idx                           = derivedTrucks.IndexOf(truck);
        if (idx != -1) derivedTrucks[idx] = derivedTruck;
    }

    private static void UpdateRelocationStatus(IDictionary<Truck, RelocationRequest> derivedRequests, Truck derivedTruck, RelocationRequest relocation)
    {
        derivedRequests[derivedTruck] = relocation with
        {
            Status = RelocationStatus.Arrived,
        };
    }

    private static void UpdateAssignments(Dictionary<TruckOperator, Truck> derivedAssignments, Truck truck, Truck derivedTruck)
    {
        if (!derivedAssignments.ContainsValue(truck)) return;
        var driver = derivedAssignments.FirstOrDefault(x => x.Value == truck).Key;
        derivedAssignments.Remove(driver);
        derivedAssignments[driver] = derivedTruck;
    }

    private static double CalculateFuelCost(double distance, double consumptionPer100Km)
    {
        var literUsed = distance * (consumptionPer100Km / 100);
        var fuelCost  = literUsed * FuelPricePerLiter;
        return fuelCost;
    }
}