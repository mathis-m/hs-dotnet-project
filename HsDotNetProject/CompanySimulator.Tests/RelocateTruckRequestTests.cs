using Common.BngUtils;
using Common.Models;
using CompanySimulator.Factories;
using CompanySimulator.Models;
using CompanySimulator.State;
using CompanySimulator.State.Actions;
using CompanySimulator.State.Reducers;
using FreightMarket.Models;
using NUnit.Framework;
using TruckDriver.Models;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Tests;

public class RelocateTruckRequestTests
{
    private static readonly Location TargetLocation = new("Amsterdam");

    private readonly Truck _assignedTruck         = new TankerTruck(Size.Small, new Age(5), new Location("Berlin"));
    private readonly Truck _assignedTruckAtTarget = new TankerTruck(Size.Small, new Age(5), TargetLocation);

    private readonly TruckOperator _driver = new(
        new Name("Some", "Name"),
        new SalaryExpectation(1, new Currency("x", "x")),
        new DriverCategory("some cat")
    );

    private readonly TruckOperator _driver2 = new(
        new Name("other", "Name"),
        new SalaryExpectation(1, new Currency("x", "x")),
        new DriverCategory("some cat")
    );

    private readonly Truck _notOwnedTruck                = new RefrigeratedTruck(Size.Small, new Age(5), new Location("Esslingen"));
    private readonly Truck _truckWithExistingRelocation  = new RefrigeratedTruck(Size.Small, new Age(5), new Location("Rom"));
    private readonly Truck _truckWithExistingRelocation2 = new RefrigeratedTruck(Size.Small, new Age(5), new Location("Lissabon"));
    private readonly Truck _unassignedTruck              = new FlatbedTruck(Size.Small, new Age(5), new Location("Istanbul"));
    private readonly Truck _unassignedTruckAtTarget      = new FlatbedTruck(Size.Small, new Age(5), TargetLocation);

    private RootState State { get; set; } = null!;
    private TruckRelocationRequestedReducer TruckRelocationRequestedReducer { get; } = new(new RelocationStatsFactory(new BngDistanceCalculator()));

    [SetUp]
    public void Setup()
    {
        State = new RootState(
            new SimulationState(
                DateTime.Now,
                new List<TransportationTender>(),
                new List<TruckOperator>(),
                new List<Truck> { _notOwnedTruck }
            ),
            new CompanyState(
                "Test",
                new AccountBalance(1, new Currency("EUR", "€")),
                new List<Truck> { _assignedTruck, _assignedTruckAtTarget, _unassignedTruck, _unassignedTruckAtTarget, _truckWithExistingRelocation, _truckWithExistingRelocation2 },
                new List<TruckOperator> { _driver },
                new List<TransportationTender>(),
                new Dictionary<TruckOperator, Truck>
                {
                    [_driver]  = _assignedTruck,
                    [_driver2] = _assignedTruckAtTarget,
                },
                new Dictionary<Truck, RelocationRequest>
                {
                    [_truckWithExistingRelocation]  = new(new Location("Esslingen"), RelocationStatus.WaitingForDriver, null, null),
                    [_truckWithExistingRelocation2] = new(new Location("Rom"), RelocationStatus.Arrived, null, null),
                },
                new Dictionary<TransportationTender, Truck>()
            ),
            new ApplicationState(
                Pages.CompanyNamePrompter
            )
        );
    }

    [Test]
    public void TruckRelocationRequestedReducer_ShouldThrow_IfTruckIsNotOwned()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // prepare
            var payload = new RequestTruckRelocationPayload(_notOwnedTruck, TargetLocation);

            // act
            var _ = TruckRelocationRequestedReducer.Reduce(State, payload);
        });
    }

    [Test]
    public void TruckRelocationRequestedReducer_ShouldThrow_IfOngoingRelocationExists()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // prepare
            var payload = new RequestTruckRelocationPayload(_truckWithExistingRelocation, TargetLocation);

            // act
            var _ = TruckRelocationRequestedReducer.Reduce(State, payload);
        });
    }

    [Test]
    public void ReducingState_WithTruckRelocationRequestedReducer_ShouldSetRelocationTarget()
    {
        // prepare
        var payload = new RequestTruckRelocationPayload(_assignedTruck, TargetLocation);

        // act
        var newState = TruckRelocationRequestedReducer.Reduce(State, payload);


        Assert.That(newState.CompanyState.TruckRelocationRequests[payload.Truck].TargetLocation, Is.EqualTo(TargetLocation));
    }

    [Test]
    public void ReducingState_WithTruckRelocationRequestedReducer_WithExistingRequestInStatusArrivedIsPermitted()
    {
        // prepare
        var payload = new RequestTruckRelocationPayload(_truckWithExistingRelocation2, TargetLocation);


        // act
        var newState = TruckRelocationRequestedReducer.Reduce(State, payload);


        Assert.That(newState.CompanyState.TruckRelocationRequests[payload.Truck].TargetLocation, Is.EqualTo(TargetLocation));
    }


    [Test]
    public void ReducingState_WithTruckRelocationRequestedReducer_SetsStatusToArrived_IfLocationsAreEqualAndTruckIsAssigned()
    {
        // prepare
        var payload = new RequestTruckRelocationPayload(_assignedTruckAtTarget, TargetLocation);


        // act
        var newState = TruckRelocationRequestedReducer.Reduce(State, payload);


        Assert.That(newState.CompanyState.TruckRelocationRequests[payload.Truck].Status, Is.EqualTo(RelocationStatus.Arrived));
    }

    [Test]
    public void ReducingState_WithTruckRelocationRequestedReducer_SetsStatusToArrived_IfLocationsAreEqualAndTruckIsUnassigned()
    {
        // prepare
        var payload = new RequestTruckRelocationPayload(_unassignedTruckAtTarget, TargetLocation);


        // act
        var newState = TruckRelocationRequestedReducer.Reduce(State, payload);


        Assert.That(newState.CompanyState.TruckRelocationRequests[payload.Truck].Status, Is.EqualTo(RelocationStatus.Arrived));
    }

    [Test]
    public void ReducingState_WithTruckRelocationRequestedReducer_SetsStatusToWaitingForDriver_IfTruckIsUnassignedAndNotAtTargetLocation()
    {
        // prepare
        var payload = new RequestTruckRelocationPayload(_unassignedTruck, TargetLocation);


        // act
        var newState = TruckRelocationRequestedReducer.Reduce(State, payload);


        Assert.That(newState.CompanyState.TruckRelocationRequests[payload.Truck].Status, Is.EqualTo(RelocationStatus.WaitingForDriver));
    }

    [Test]
    public void ReducingState_WithTruckRelocationRequestedReducer_SetsStatusToRelocationStarted_IfTruckIsAssignedAndNotAtTargetLocation()
    {
        // prepare
        var payload = new RequestTruckRelocationPayload(_assignedTruck, TargetLocation);


        // act
        var newState = TruckRelocationRequestedReducer.Reduce(State, payload);


        Assert.That(newState.CompanyState.TruckRelocationRequests[payload.Truck].Status, Is.EqualTo(RelocationStatus.RelocationStarted));
    }
}