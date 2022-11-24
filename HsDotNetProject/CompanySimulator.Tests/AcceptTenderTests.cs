using Common.Models;
using CompanySimulator.Models;
using CompanySimulator.State;
using CompanySimulator.State.Reducers;
using FreightMarket.Models;
using NUnit.Framework;
using TruckDriver.Models;
using VehicleAcquisition.Factories;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Tests;

public class AcceptTenderTests
{
    private const double InitialBalance = 50000;

    private static readonly Currency Currency = new("x", "x");

    private readonly TransportationTender _tenderToAccept = new(
        new TransportationGoods(GoodTypes.Chocolate, TruckTypes.RefrigeratedTruck, 10, new GoodPrice(10, Currency)),
        new Location("a"),
        new Location("b"),
        DateTime.Now,
        new TenderCompensation(1, Currency),
        new TenderPenalty(1, Currency)
    );

    private RootState State { get; set; } = null!;
    private TenderAcceptedReducer TenderAcceptedReducer { get; } = new();

    [SetUp]
    public void Setup()
    {
        State = new RootState(
            new SimulationState(
                DateTime.Now,
                new List<TransportationTender> { _tenderToAccept },
                new List<TruckOperator>(),
                new List<Truck>()
            ),
            new CompanyState(
                "Test",
                new AccountBalance(InitialBalance, Currency),
                new List<Truck>(),
                new List<TruckOperator>(),
                new List<TransportationTender>()
            ),
            new ApplicationState(
                Pages.CompanyNamePrompter
            )
        );
    }

    [Test]
    public void ReducingState_WithTenderAcceptedReducer_AddsTenderToCompanyState()
    {
        // act
        var newState = TenderAcceptedReducer.Reduce(State, _tenderToAccept);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.AcceptedTenderCount, Is.EqualTo(1));
        Assert.That(newState.CompanyState.AcceptedTenders.Count, Is.EqualTo(1));
        Assert.That(newState.CompanyState.AcceptedTenders.First(), Is.EqualTo(_tenderToAccept));
    }

    [Test]
    public void ReducingState_WithTenderAcceptedReducer_RemovesTenderFromSimulationState()
    {
        // act
        var newState = TenderAcceptedReducer.Reduce(State, _tenderToAccept);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.SimulationState.OpenTenders.Count, Is.EqualTo(0));
    }
}