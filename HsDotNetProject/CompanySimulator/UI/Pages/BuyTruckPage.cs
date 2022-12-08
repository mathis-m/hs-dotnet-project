using CompanySimulator.State;
using CompanySimulator.State.Actions;
using CompanySimulator.UI.Utils.TableUtils;
using UconsoleI.Components.TableComponent;
using UconsoleI.Rendering;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.UI.Pages;

public class BuyTruckPage : BaseTablePage<Truck>
{
    public BuyTruckPage(StateManager stateManager) : base(stateManager)
    {
    }

    protected override string Prompt => "Buy a truck";

    protected override List<TableColumn> TableColumns => TruckTableUtil.TableColumns;

    protected override IEnumerable<IComponent> GetTableRow(int num, Truck truck)
    {
        return TruckTableUtil.GetTableRow(num, truck);
    }

    protected override IReadOnlyList<Truck> GetSimulationList(RootState state)
    {
        return state.SimulationState.AvailableTrucks;
    }

    protected override ActionWithPayload<Truck> GetOnSelectedAction(Truck truck)
    {
        return new BuyTruckAction(truck);
    }
}