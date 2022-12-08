using Common.Models;
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

public class AssignDriverToTruckTests
{
    private readonly Truck _currentAssignedTruck = new FlatbedTruck(Size.Small, new Age(5), new Location("some other"));

    private readonly TruckOperator _currentlyAssignedDriver = new(
        new Name("Other", "Name"),
        new SalaryExpectation(1, new Currency("x", "x")),
        new DriverCategory("some other cat")
    );

    private readonly TruckOperator _driverToAssign = new(
        new Name("Some", "Name"),
        new SalaryExpectation(1, new Currency("x", "x")),
        new DriverCategory("some cat")
    );

    private readonly TruckOperator _notEmployedDriver = new(
        new Name("Don't want", "To work for your company"),
        new SalaryExpectation(1, new Currency("x", "x")),
        new DriverCategory("some other cat")
    );

    private readonly Truck _notOwnedTruck = new RefrigeratedTruck(Size.Small, new Age(5), new Location("somewhere else"));

    private readonly Truck _truckToAssignTo = new TankerTruck(Size.Small, new Age(5), new Location("some"));

    private RootState State { get; set; } = null!;
    private DriverAssignedToTruckReducer DriverAssignedToTruckReducer { get; } = new();

    [SetUp]
    public void Setup()
    {
        State = new RootState(
            new SimulationState(
                DateTime.Now,
                new List<TransportationTender>(),
                new List<TruckOperator> { _notEmployedDriver },
                new List<Truck> { _notOwnedTruck }
            ),
            new CompanyState(
                "Test",
                new AccountBalance(1, new Currency("EUR", "€")),
                new List<Truck> { _truckToAssignTo, _currentAssignedTruck },
                new List<TruckOperator> { _driverToAssign, _currentlyAssignedDriver },
                new List<TransportationTender>(),
                new Dictionary<TruckOperator, Truck>
                {
                    [_currentlyAssignedDriver] = _currentAssignedTruck,
                },
                new Dictionary<Truck, RelocationRequest>(),
                new Dictionary<TransportationTender, Truck>()
            ),
            new ApplicationState(
                Pages.CompanyNamePrompter
            )
        );
    }

    [Test]
    public void DriverAssignedToTruckReducer_ShouldThrow_WhenDriverIsNotEmployed()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // act
            var _ = DriverAssignedToTruckReducer.Reduce(State, new AssignDriverToTruckPayload(_notEmployedDriver, _truckToAssignTo));
        });
    }


    [Test]
    public void DriverAssignedToTruckReducer_ShouldThrow_WhenTruckIsNotOwned()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // act
            var _ = DriverAssignedToTruckReducer.Reduce(State, new AssignDriverToTruckPayload(_driverToAssign, _notOwnedTruck));
        });
    }


    [Test]
    public void ReducingState_WithDriverAssignedToTruckReducer_AddsAssignment()
    {
        // prepare
        var payload = new AssignDriverToTruckPayload(_driverToAssign, _truckToAssignTo);

        // act
        var newState = DriverAssignedToTruckReducer.Reduce(State, payload);


        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.TruckAssignments[payload.Driver], Is.EqualTo(payload.Truck));
    }

    [Test]
    public void ReducingState_WithDriverAssignedToTruckReducer_WillReplaceTheCurrentAssignedTruck()
    {
        // prepare
        var payload = new AssignDriverToTruckPayload(_currentlyAssignedDriver, _truckToAssignTo);

        // act
        var newState = DriverAssignedToTruckReducer.Reduce(State, payload);


        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.TruckAssignments[payload.Driver], Is.EqualTo(payload.Truck));
    }

    [Test]
    public void ReducingState_WithDriverAssignedToTruckReducer_WithAlreadyAssignedTruck_WillRemoveTheOldAssignment()
    {
        // prepare
        var payload = new AssignDriverToTruckPayload(_driverToAssign, _currentAssignedTruck);

        // act
        var newState = DriverAssignedToTruckReducer.Reduce(State, payload);


        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.TruckAssignments.Values.Count(truck => truck == payload.Truck), Is.EqualTo(1));
        Assert.That(newState.CompanyState.TruckAssignments.First(x => x.Value == payload.Truck).Key, Is.EqualTo(payload.Driver));
    }
}