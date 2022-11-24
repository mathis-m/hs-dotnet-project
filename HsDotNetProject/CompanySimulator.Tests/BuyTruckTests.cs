using Common.Models;
using CompanySimulator.Models;
using CompanySimulator.State;
using CompanySimulator.State.Reducers;
using FreightMarket.Models;
using NUnit.Framework;
using TruckDriver.Models;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.Tests;

public class BuyTruckTests
{
    private const double InitialBalance = 50000;

    private readonly Truck _truckToBuy = new TankerTruck(Size.ExtraLarge, new Age(5), new Location("Somewhere"));

    private RootState State { get; set; } = null!;
    private TruckBoughtReducer TruckBoughtReducer { get; } = new();

    [SetUp]
    public void Setup()
    {
        State = new RootState(
            new SimulationState(
                DateTime.Now,
                new List<TransportationTender>(),
                new List<TruckOperator>(),
                new List<Truck> { _truckToBuy }
            ),
            new CompanyState(
                "Test",
                new AccountBalance(InitialBalance, new Currency("EUR", "€")),
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
    public void ReducingState_WithTruckBoughtReducer_AddsTruckToCompanyState()
    {
        // act
        var newState = TruckBoughtReducer.Reduce(State, _truckToBuy);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.OwnedTruckCount, Is.EqualTo(1));
        Assert.That(newState.CompanyState.OwnedTrucks.Count, Is.EqualTo(1));
        Assert.That(newState.CompanyState.OwnedTrucks.First(), Is.EqualTo(_truckToBuy));
    }

    [Test]
    public void ReducingState_WithTruckBoughtReducer_RemovesTruckFromSimulationState()
    {
        // act
        var newState = TruckBoughtReducer.Reduce(State, _truckToBuy);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.SimulationState.AvailableTrucks.Count, Is.EqualTo(0));
    }

    [Test]
    public void ReducingState_WithTruckBoughtReducer_SubtractsTruckPriceFromBalance()
    {
        // act
        var newState = TruckBoughtReducer.Reduce(State, _truckToBuy);

        // assert
        Assert.That(newState, Is.Not.Null);
        Assert.That(newState.CompanyState.AccountBalance.Balance, Is.EqualTo(InitialBalance - _truckToBuy.Price.Value));
    }
}