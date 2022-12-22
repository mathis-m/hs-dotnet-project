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
using VehicleAcquisition.Factories;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Tests;

public class AssignTenderToTruckTests
{
    private static readonly Currency Currency = new("x", "x");

    private static readonly Location StartLocation  = new("start");
    private static readonly Location TargetLocation = new("target");

    private readonly TransportationTender _acceptedTender = new(
        new TransportationGoods(GoodTypes.Chocolate, TruckTypes.RefrigeratedTruck, 1, new GoodPrice(10, Currency)),
        StartLocation,
        new Location("2"),
        DateTime.Now,
        new TenderCompensation(1, Currency),
        new TenderPenalty(1, Currency)
    );

    private readonly TransportationTender _assignedTender = new(
        new TransportationGoods(GoodTypes.Chocolate, TruckTypes.RefrigeratedTruck, 1, new GoodPrice(10, Currency)),
        StartLocation,
        TargetLocation,
        DateTime.Now,
        new TenderCompensation(1, Currency),
        new TenderPenalty(1, Currency)
    );

    private readonly TruckOperator _driver = new(
        new Name("other", "Name"),
        new SalaryExpectation(1, new Currency("x", "x")),
        new DriverCategory("some cat")
    );

    private readonly Truck _notOwnedTruck = new RefrigeratedTruck(Size.Small, new Age(5), StartLocation);

    private readonly TransportationTender _openTender = new(
        new TransportationGoods(GoodTypes.Chocolate, TruckTypes.RefrigeratedTruck, 1, new GoodPrice(10, Currency)),
        StartLocation,
        new Location("4"),
        DateTime.Now,
        new TenderCompensation(1, Currency),
        new TenderPenalty(1, Currency)
    );


    private readonly Truck _ownedTruck                 = new RefrigeratedTruck(Size.ExtraLarge, new Age(1), StartLocation);
    private readonly Truck _truckAssignedToTender      = new RefrigeratedTruck(Size.Small, new Age(5), TargetLocation);
    private readonly Truck _truckWithRelocation        = new RefrigeratedTruck(Size.Small, new Age(5), StartLocation);
    private readonly Truck _truckWithRelocationArrived = new RefrigeratedTruck(Size.Small, new Age(5), StartLocation);

    private readonly TransportationTender _veryLargeGoodWeight = new(
        new TransportationGoods(GoodTypes.Chocolate, TruckTypes.RefrigeratedTruck, int.MaxValue, new GoodPrice(10, Currency)),
        StartLocation,
        new Location("5"),
        DateTime.Now,
        new TenderCompensation(1, Currency),
        new TenderPenalty(1, Currency)
    );

    private readonly Truck _wrongLocation  = new RefrigeratedTruck(Size.Small, new Age(5), new Location("some other location"));
    private readonly Truck _wrongTypeTruck = new FlatbedTruck(Size.Small, new Age(5), StartLocation);

    private RootState State { get; set; } = null!;
    private TenderAssignedToTruckReducer TenderAssignedToTruckReducer { get; } = new(new TruckRelocationRequestedReducer(new RelocationStatsFactory(new BngDistanceCalculator())));

    [SetUp]
    public void Setup()
    {
        State = new RootState(
            new SimulationState(
                DateTime.Now,
                new List<TransportationTender> { _openTender },
                new List<TruckOperator>(),
                new List<Truck> { _notOwnedTruck }
            ),
            new CompanyState(
                "Test",
                new AccountBalance(1, new Currency("EUR", "€")),
                new List<Truck> { _ownedTruck, _truckWithRelocation, _truckAssignedToTender, _wrongTypeTruck, _wrongLocation, _truckWithRelocationArrived },
                new List<TruckOperator> { _driver },
                new List<TransportationTender> { _acceptedTender, _assignedTender, _veryLargeGoodWeight },
                new Dictionary<TruckOperator, Truck>(),
                new Dictionary<Truck, RelocationRequest>
                {
                    [_truckWithRelocation]        = new(new Location("some other location"), RelocationStatus.WaitingForDriver, null, null),
                    [_truckWithRelocationArrived] = new(StartLocation, RelocationStatus.Arrived, null, null),
                },
                new Dictionary<TransportationTender, Truck>
                {
                    [_assignedTender] = _truckAssignedToTender,
                }
            ),
            new ApplicationState(
                Pages.CompanyNamePrompter
            )
        );
    }

    [Test]
    public void TenderAssignedToTruckReducer_ShouldThrow_IfTenderIsNotAccepted()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // prepare
            var payload = new AssignTenderToTruckPayload(_openTender, _ownedTruck);

            // act
            var _ = TenderAssignedToTruckReducer.Reduce(State, payload);
        });
    }

    [Test]
    public void TenderAssignedToTruckReducer_ShouldThrow_IfTruckIsNotOwned()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // prepare
            var payload = new AssignTenderToTruckPayload(_acceptedTender, _notOwnedTruck);

            // act
            var _ = TenderAssignedToTruckReducer.Reduce(State, payload);
        });
    }

    [Test]
    public void TenderAssignedToTruckReducer_ShouldThrow_IfTruckIsAssignedToAnotherTender()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // prepare
            var payload = new AssignTenderToTruckPayload(_acceptedTender, _truckAssignedToTender);

            // act
            var _ = TenderAssignedToTruckReducer.Reduce(State, payload);
        });
    }

    [Test]
    public void TenderAssignedToTruckReducer_ShouldThrow_IfTruckIsInAnOngoingRelocation()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // prepare
            var payload = new AssignTenderToTruckPayload(_acceptedTender, _truckWithRelocation);

            // act
            var _ = TenderAssignedToTruckReducer.Reduce(State, payload);
        });
    }

    [Test]
    public void TenderAssignedToTruckReducer_ShouldThrow_IfTenderIsNotValid()
    {
        // TODO: tbd what valid means
        const bool actual = true;
        Assert.That(actual, Is.True);
    }

    [Test]
    public void TenderAssignedToTruckReducer_ShouldThrow_IfTenderIsAlreadyAssigned()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // prepare
            var payload = new AssignTenderToTruckPayload(_assignedTender, _ownedTruck);

            // act
            var _ = TenderAssignedToTruckReducer.Reduce(State, payload);
        });
    }

    [Test]
    public void TenderAssignedToTruckReducer_ShouldThrow_IfGoodDoesNotMatchTruckType()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // prepare
            var payload = new AssignTenderToTruckPayload(_acceptedTender, _wrongTypeTruck);

            // act
            var _ = TenderAssignedToTruckReducer.Reduce(State, payload);
        });
    }

    [Test]
    public void TenderAssignedToTruckReducer_ShouldThrow_IfGoodWeightIsMoreThanMaxPayload()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // prepare
            var payload = new AssignTenderToTruckPayload(_veryLargeGoodWeight, _ownedTruck);

            // act
            var _ = TenderAssignedToTruckReducer.Reduce(State, payload);
        });
    }

    [Test]
    public void TenderAssignedToTruckReducer_ShouldThrow_IfLocationsAreNotMatching()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            // prepare
            var payload = new AssignTenderToTruckPayload(_acceptedTender, _wrongLocation);

            // act
            var _ = TenderAssignedToTruckReducer.Reduce(State, payload);
        });
    }


    [Test]
    public void ReducingState_WithTenderAssignedToTruckReducer_ShouldCreateRelocationRequests()
    {
        // prepare
        var payload = new AssignTenderToTruckPayload(_acceptedTender, _ownedTruck);

        // act
        var newState = TenderAssignedToTruckReducer.Reduce(State, payload);

        // Assert
        Assert.That(newState.CompanyState.TruckRelocationRequests[payload.Truck].TargetLocation, Is.EqualTo(payload.Tender.TargetLocation));
    }

    [Test]
    public void ReducingState_WithTenderAssignedToTruckReducer_ShouldUpdateRelocationRequests_IfItWasInStateArrived()
    {
        // prepare
        var payload = new AssignTenderToTruckPayload(_acceptedTender, _truckWithRelocationArrived);

        // act
        var newState = TenderAssignedToTruckReducer.Reduce(State, payload);

        // Assert
        Assert.That(newState.CompanyState.TruckRelocationRequests[payload.Truck].TargetLocation, Is.EqualTo(payload.Tender.TargetLocation));
    }

    [Test]
    public void ReducingState_WithTenderAssignedToTruckReducer_ShouldUpdateTenderAssignments()
    {
        // prepare
        var payload = new AssignTenderToTruckPayload(_acceptedTender, _ownedTruck);

        // act
        var newState = TenderAssignedToTruckReducer.Reduce(State, payload);

        // Assert
        Assert.That(newState.CompanyState.TenderAssignments[payload.Tender], Is.EqualTo(payload.Truck));
    }
}