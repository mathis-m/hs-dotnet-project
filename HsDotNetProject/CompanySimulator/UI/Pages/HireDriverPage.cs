using CompanySimulator.State;
using CompanySimulator.State.Actions;
using TruckDriver.Models;
using UconsoleI.Components.TableComponent;
using UconsoleI.Components.TextComponent;
using UconsoleI.Extensions;
using UconsoleI.Rendering;

namespace CompanySimulator.UI.Pages;

public class HireDriverPage : BaseTablePage<TruckOperator>
{
    public HireDriverPage(StateManager stateManager) : base(stateManager)
    {
    }

    protected override string Prompt => "Hire a truck driver";

    protected override List<TableColumn> TableColumns => new()
    {
        new TableColumn("#", Justify.Right),
        new TableColumn("Name", Justify.Center),
        new TableColumn("Salary", Justify.Center),
        new TableColumn("Type", Justify.Center),
    };

    protected override IReadOnlyList<TruckOperator> GetSimulationList(RootState state)
    {
        return state.SimulationState.JobSeekingTruckDrivers;
    }

    protected override ActionWithPayload<TruckOperator> GetOnSelectedAction(TruckOperator driver)
    {
        return new HireDriverAction(driver);
    }

    protected override IEnumerable<IComponent> GetTableRow(int num, TruckOperator driver)
    {
        return new List<IComponent>
        {
            new Text($"{num}")
                .RightAligned(),
            new Text(driver.Name.FullName)
                .Overflow(Overflow.Wrap),
            new Text(driver.SalaryExpectation.FormattedValueWithIsoCode)
                .RightAligned()
                .Overflow(Overflow.Wrap),
            new Text(driver.DriverCategory.Type)
                .Overflow(Overflow.Wrap),
        };
    }
}