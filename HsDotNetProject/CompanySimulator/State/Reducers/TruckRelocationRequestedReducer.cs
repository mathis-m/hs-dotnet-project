using CompanySimulator.Factories;
using CompanySimulator.Models;
using CompanySimulator.State.Actions;
using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Reducers;

public class TruckRelocationRequestedReducer : IReducerT<RequestTruckRelocationPayload>
{
    private readonly IRelocationStatsFactory _statsFactory;

    public TruckRelocationRequestedReducer(IRelocationStatsFactory statsFactory)
    {
        _statsFactory = statsFactory;
    }

    public RootState Reduce(RootState currentState, RequestTruckRelocationPayload payload)
    {
        if (!currentState.CompanyState.OwnedTrucks.Contains(payload.Truck)) throw new InvalidOperationException("Cannot place relocation request on a truck that is not owned");

        var currentRelocationRequests = currentState.CompanyState.TruckRelocationRequests;

        if (currentRelocationRequests.ContainsKey(payload.Truck))
        {
            var ongoingRelocation = currentRelocationRequests[payload.Truck];

            if (ongoingRelocation is { Status: not RelocationStatus.Arrived }) throw new InvalidOperationException("Cannot relocate truck with ongoing relocation");
        }


        var truckAssignments = new Dictionary<TruckOperator, Truck>(currentState.CompanyState.TruckAssignments);

        var hasAssignedDriver = truckAssignments.ContainsValue(payload.Truck);

        var relocationStatus = RelocationStatus.RelocationStarted;
        if (payload.Truck.Location == payload.Location)
            relocationStatus                          = RelocationStatus.Arrived;
        else if (!hasAssignedDriver) relocationStatus = RelocationStatus.WaitingForDriver;


        var stats = !hasAssignedDriver
            ? null
            : _statsFactory.Create(payload.Truck, truckAssignments.FirstOrDefault(x => x.Value == payload.Truck).Key, payload.Location, payload.PayloadInTons);

        DateTime? estimatedArrival = stats == null ? null : _statsFactory.CalculateArrival(stats, currentState.SimulationState.SimulationDate);

        var derivedRelocationRequests = new Dictionary<Truck, RelocationRequest>(currentRelocationRequests)
        {
            [payload.Truck] = new(payload.Location, relocationStatus, estimatedArrival, stats),
        };

        var derivedCompanyState = currentState.CompanyState with
        {
            TruckRelocationRequests = derivedRelocationRequests,
        };

        return currentState with
        {
            CompanyState = derivedCompanyState,
        };
    }
}