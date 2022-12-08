using CompanySimulator.State;
using CompanySimulator.State.Actions;
using CompanySimulator.UI.Utils.TableUtils;
using TruckDriver.Models;
using UconsoleI.Components.TableComponent;
using UconsoleI.Rendering;
using UconsoleI.Stylings;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.UI.Pages;

public class AssignDriverToTruckPage : BaseTableCombinerPage<TruckOperator, Truck, AssignDriverToTruckAction, AssignDriverToTruckPayload>
{
    private static readonly Styling TitleStyle = new(decoration: Decoration.Bold | Decoration.Underline);


    private string _currentDriverName = string.Empty;

    public AssignDriverToTruckPage(StateManager stateManager) : base(stateManager)
    {
    }

    protected override Action<TruckOperator>? OnFirstSelected => OnDriverSelected;

    protected override TableTitle? FirstTitle { get; } = new("Select a Driver that you want to assign to a truck", TitleStyle);
    protected override TableTitle? SecondTitle => new($"Select a truck for {_currentDriverName}", TitleStyle);

    protected override string FirstPrompt => "Select Driver to assign";
    protected override string SecondPrompt => "Select Truck for driver";
    protected override List<TableColumn> FirstTableColumns => DriverTableUtil.TableColumns;
    protected override List<TableColumn> SecondTableColumns => TruckTableUtil.TableColumns;

    private void OnDriverSelected(TruckOperator driver)
    {
        _currentDriverName = driver.Name.FullName;
    }

    protected override IReadOnlyList<TruckOperator> GetFirstList(RootState state)
    {
        return state.CompanyState.Employees;
    }

    protected override IReadOnlyList<Truck> GetSecondList(RootState state)
    {
        return state.CompanyState.OwnedTrucks;
    }

    protected override AssignDriverToTruckAction CreateActionOnBothSelected(TruckOperator driver, Truck truck)
    {
        return new AssignDriverToTruckAction(new AssignDriverToTruckPayload(driver, truck));
    }

    protected override IEnumerable<IComponent> GetRowOfFirstTable(int num, TruckOperator driver)
    {
        return DriverTableUtil.GetTableRow(num, driver);
    }

    protected override IEnumerable<IComponent> GetRowOfSecondTable(int num, Truck truck)
    {
        return TruckTableUtil.GetTableRow(num, truck);
    }
}