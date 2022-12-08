using CompanySimulator.State;
using CompanySimulator.State.Actions;
using CompanySimulator.UI.Utils.TableUtils;
using TruckDriver.Models;
using UconsoleI.Components.TableComponent;
using UconsoleI.Rendering;

namespace CompanySimulator.UI.Pages;

public class HireDriverPage : BaseTablePage<TruckOperator>
{
    public HireDriverPage(StateManager stateManager) : base(stateManager)
    {
    }

    protected override string Prompt => "Hire a truck driver";

    protected override List<TableColumn> TableColumns => DriverTableUtil.TableColumns;

    protected override IEnumerable<IComponent> GetTableRow(int num, TruckOperator driver)
    {
        return DriverTableUtil.GetTableRow(num, driver);
    }

    protected override IReadOnlyList<TruckOperator> GetSimulationList(RootState state)
    {
        return state.SimulationState.JobSeekingTruckDrivers;
    }

    protected override ActionWithPayload<TruckOperator> GetOnSelectedAction(TruckOperator driver)
    {
        return new HireDriverAction(driver);
    }
}