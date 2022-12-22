using VehicleAcquisition.Models;

namespace CompanySimulator.Models;

public record RelocationRequest(Location TargetLocation, RelocationStatus Status, DateTime? EstimatedArrival, RelocationStats? Stats);