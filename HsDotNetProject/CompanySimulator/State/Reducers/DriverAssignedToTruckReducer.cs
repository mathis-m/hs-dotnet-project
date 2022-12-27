using CompanySimulator.Factories;
using CompanySimulator.Models;
using CompanySimulator.State.Actions;
using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Reducers;

public class DriverAssignedToTruckReducer : IReducerT<AssignDriverToTruckPayload>
{
    private readonly IRelocationStatsFactory _statsFactory;

    public DriverAssignedToTruckReducer(IRelocationStatsFactory statsFactory)
    {
        _statsFactory = statsFactory;
    }

    public RootState Reduce(RootState currentState, AssignDriverToTruckPayload payload)
    {
        if (!currentState.CompanyState.Employees.Contains(payload.Driver)) throw new InvalidOperationException("Cannot assign a driver that is not employed");

        if (!currentState.CompanyState.OwnedTrucks.Contains(payload.Truck)) throw new InvalidOperationException("Cannot assign driver to a not owned truck");

        if (currentState.CompanyState.TruckRelocationRequests.ContainsKey(payload.Truck) && currentState.CompanyState.TruckRelocationRequests[payload.Truck] is
                { Status: not RelocationStatus.WaitingForDriver })
            throw new InvalidOperationException("Cannot assign driver to a truck with ongoing relocation");

        var currentAssignments = currentState.CompanyState.TruckAssignments;
        var derivedAssignments = new Dictionary<TruckOperator, Truck>(currentAssignments);

        if (derivedAssignments.ContainsValue(payload.Truck))
        {
            var (oldDriver, _) = derivedAssignments.First(x => x.Value == payload.Truck);

            derivedAssignments.Remove(oldDriver);
        }

        derivedAssignments[payload.Driver] = payload.Truck;


        var derivedRelocationRequests = new Dictionary<Truck, RelocationRequest>(currentState.CompanyState.TruckRelocationRequests);


        if (derivedRelocationRequests.ContainsKey(payload.Truck))
        {
            var request          = derivedRelocationRequests[payload.Truck];
            var stats            = _statsFactory.Create(payload.Truck, payload.Driver, request.TargetLocation, 0);
            var estimatedArrival = _statsFactory.CalculateArrival(stats, currentState.SimulationState.SimulationDate);
            derivedRelocationRequests[payload.Truck] = request with
            {
                Status = RelocationStatus.RelocationStarted,
                EstimatedArrival = estimatedArrival,
                Stats = stats,
            };
        }

        var derivedCompanyState = currentState.CompanyState with
        {
            TruckAssignments = derivedAssignments,
            TruckRelocationRequests = derivedRelocationRequests,
        };

        return currentState with
        {
            CompanyState = derivedCompanyState,
        };
    }
}