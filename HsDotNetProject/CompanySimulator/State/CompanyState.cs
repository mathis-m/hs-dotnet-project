using CompanySimulator.Models;
using FreightMarket.Models;
using TruckDriver.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.State;

public record CompanyState(
    string Name,
    AccountBalance AccountBalance,
    IReadOnlyList<Truck> OwnedTrucks,
    IReadOnlyList<TruckOperator> Employees,
    IReadOnlyList<TransportationTender> AcceptedTenders,
    IReadOnlyDictionary<TruckOperator, Truck> TruckAssignments,
    IReadOnlyDictionary<Truck, RelocationRequest> TruckRelocationRequests,
    IReadOnlyDictionary<TransportationTender, Truck> TenderAssignments
)
{
    public int OwnedTruckCount => OwnedTrucks.Count;
    public int EmployeeCount => Employees.Count;
    public int AcceptedTenderCount => AcceptedTenders.Count;
}