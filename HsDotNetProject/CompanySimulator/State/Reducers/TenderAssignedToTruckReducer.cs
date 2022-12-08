using CompanySimulator.Models;
using CompanySimulator.State.Actions;
using FreightMarket.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State.Reducers;

public class TenderAssignedToTruckReducer : IReducerT<AssignTenderToTruckPayload>
{
    private readonly TruckRelocationRequestedReducer _relocationReducer;

    public TenderAssignedToTruckReducer(TruckRelocationRequestedReducer relocationReducer)
    {
        _relocationReducer = relocationReducer;
    }

    public RootState Reduce(RootState currentState, AssignTenderToTruckPayload payload)
    {
        var currentAssignments = currentState.CompanyState.TenderAssignments;

        var currentRelocationRequests = currentState.CompanyState.TruckRelocationRequests;

        var ongoingRelocation = currentRelocationRequests.ContainsKey(payload.Truck) ? currentRelocationRequests[payload.Truck] : null;

        // TODO: check that tender is valid, tbd what valid means
        ValidatePayload(currentState, payload, currentAssignments, ongoingRelocation);


        var derivedAssignments = AddOrReassignTruckToTender(payload, currentAssignments);

        var derivedRelocationRequests = ReduceRelocationRequests(currentState, payload);

        var derivedCompanyState = currentState.CompanyState with
        {
            TenderAssignments = derivedAssignments,
            TruckRelocationRequests = derivedRelocationRequests,
        };

        return currentState with
        {
            CompanyState = derivedCompanyState,
        };
    }

    private IReadOnlyDictionary<Truck, RelocationRequest> ReduceRelocationRequests(RootState currentState, AssignTenderToTruckPayload payload)
    {
        var derivedRelocationState = _relocationReducer.Reduce(currentState, new RequestTruckRelocationPayload(payload.Truck, payload.Tender.TargetLocation));

        var derivedRelocationRequests = derivedRelocationState.CompanyState.TruckRelocationRequests;
        return derivedRelocationRequests;
    }

    private static Dictionary<TransportationTender, Truck> AddOrReassignTruckToTender(AssignTenderToTruckPayload payload,
        IReadOnlyDictionary<TransportationTender, Truck> currentAssignments)
    {
        var derivedAssignments = new Dictionary<TransportationTender, Truck>(currentAssignments);

        if (derivedAssignments.ContainsValue(payload.Truck))
        {
            var (oldTender, _) = derivedAssignments.First(x => x.Value == payload.Truck);

            derivedAssignments.Remove(oldTender);
        }

        derivedAssignments[payload.Tender] = payload.Truck;
        return derivedAssignments;
    }

    private static void ValidatePayload(RootState currentState, AssignTenderToTruckPayload payload, IReadOnlyDictionary<TransportationTender, Truck> currentAssignments,
        RelocationRequest? ongoingRelocation)
    {
        if (!currentState.CompanyState.AcceptedTenders.Contains(payload.Tender)) throw new InvalidOperationException("Cannot assign a tender that is not accepted");

        if (!currentState.CompanyState.OwnedTrucks.Contains(payload.Truck)) throw new InvalidOperationException("Cannot assign tender to a not owned truck");


        if (currentAssignments.ContainsKey(payload.Tender)) throw new InvalidOperationException("Cannot reassign tender");

        if (!IsTruckFittingTenderRequirements(payload.Truck, payload.Tender)) throw new InvalidOperationException("Truck does not satisfy the tender requirements");

        if (ongoingRelocation is { Status: not RelocationStatus.Arrived }) throw new InvalidOperationException("Cannot assign tender to a truck with ongoing relocation");
    }

    private static bool IsTruckFittingTenderRequirements(ITruck truck, TransportationTender tender)
    {
        var matchesGoodType      = truck.TruckType == tender.TransportationGoods.TruckType;
        var matchesStartLocation = truck.Location == tender.StartLocation;
        var canCarryGoodWeight   = tender.TransportationGoods.WeightInTons <= truck.MaxPayloadInTons;

        return matchesGoodType && matchesStartLocation && canCarryGoodWeight;
    }
}