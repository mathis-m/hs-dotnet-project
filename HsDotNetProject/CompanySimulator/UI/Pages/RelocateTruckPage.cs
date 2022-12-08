using CompanySimulator.State;
using CompanySimulator.State.Actions;
using CompanySimulator.UI.Utils.TableUtils;
using UconsoleI.Components.TableComponent;
using UconsoleI.Rendering;
using UconsoleI.Stylings;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;
using VehicleAcquisition.Services;

namespace CompanySimulator.UI.Pages;

public class RelocateTruckPage : BaseTableCombinerPage<Truck, Location, RequestTruckRelocationAction, RequestTruckRelocationPayload>
{
    private static readonly Styling TitleStyle = new(decoration: Decoration.Bold | Decoration.Underline);

    private string _currentLocation = string.Empty;

    public RelocateTruckPage(StateManager stateManager) : base(stateManager)
    {
    }

    protected override Action<Truck>? OnFirstSelected => OnTruckSelected;

    protected override TableTitle? FirstTitle { get; } = new("Select a truck to relocate", TitleStyle);
    protected override TableTitle? SecondTitle => new($"Relocate from {_currentLocation} to:", TitleStyle);

    protected override string FirstPrompt => "Select truck";
    protected override string SecondPrompt => "Select location";
    protected override List<TableColumn> FirstTableColumns => TruckTableUtil.TableColumns;
    protected override List<TableColumn> SecondTableColumns => LocationTableUtil.TableColumns;

    private void OnTruckSelected(Truck truck)
    {
        _currentLocation = truck.Location.City;
    }

    protected override IReadOnlyList<Truck> GetFirstList(RootState state)
    {
        var trucks = state.CompanyState.OwnedTrucks;
        return trucks.Where(truck => !state.CompanyState.TruckRelocationRequests.ContainsKey(truck)).ToList();
    }

    protected override IReadOnlyList<Location> GetSecondList(RootState state)
    {
        return LocationRandomizerService.PossibleLocations;
    }

    protected override RequestTruckRelocationAction CreateActionOnBothSelected(Truck truck, Location location)
    {
        return new RequestTruckRelocationAction(new RequestTruckRelocationPayload(truck, location));
    }

    protected override IEnumerable<IComponent> GetRowOfFirstTable(int num, Truck truck)
    {
        return TruckTableUtil.GetTableRow(num, truck);
    }

    protected override IEnumerable<IComponent> GetRowOfSecondTable(int num, Location location)
    {
        return LocationTableUtil.GetTableRow(num, location);
    }
}