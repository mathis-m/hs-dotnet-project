using CompanySimulator.State;
using CompanySimulator.State.Actions;
using CompanySimulator.UI.Utils.TableUtils;
using FreightMarket.Models;
using UconsoleI.Components.TableComponent;
using UconsoleI.Rendering;
using UconsoleI.Stylings;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.UI.Pages;

public class AssignTenderPage : BaseTableCombinerPage<TransportationTender, Truck, AssignTenderToTruckAction, AssignTenderToTruckPayload>
{
    private static readonly Styling TitleStyle = new(decoration: Decoration.Bold | Decoration.Underline);

    private TransportationTender? _selectedTender;

    public AssignTenderPage(StateManager stateManager) : base(stateManager)
    {
    }

    protected override Action<TransportationTender>? OnFirstSelected => OnDriverSelected;


    protected override TableTitle? FirstTitle { get; } = new("Select tender to be assigned", TitleStyle);
    protected override TableTitle? SecondTitle => new("Select truck to assign tender", TitleStyle);

    protected override string FirstPrompt => "Select tender";
    protected override string SecondPrompt => "Select truck";
    protected override List<TableColumn> FirstTableColumns => TenderTableUtil.TableColumns;
    protected override List<TableColumn> SecondTableColumns => TruckTableUtil.TableColumns;

    private void OnDriverSelected(TransportationTender tender)
    {
        _selectedTender = tender;
    }

    protected override IReadOnlyList<TransportationTender> GetFirstList(RootState state)
    {
        var tenders = state.CompanyState.AcceptedTenders;
        return tenders.Where(tender => !state.CompanyState.TenderAssignments.ContainsKey(tender)).ToList();
    }

    protected override IReadOnlyList<Truck> GetSecondList(RootState state)
    {
        if (_selectedTender is null)
            return new List<Truck>();

        var trucks = state.CompanyState.OwnedTrucks;

        return trucks.Where(truck =>
        {
            var matchesGoodType      = truck.TruckType == _selectedTender.TransportationGoods.TruckType;
            var matchesStartLocation = truck.Location == _selectedTender.StartLocation;
            var canCarryGoodWeight   = _selectedTender.TransportationGoods.WeightInTons <= truck.MaxPayloadInTons;

            return matchesGoodType && matchesStartLocation && canCarryGoodWeight;
        }).ToList();
    }

    protected override AssignTenderToTruckAction CreateActionOnBothSelected(TransportationTender tender, Truck truck)
    {
        return new AssignTenderToTruckAction(new AssignTenderToTruckPayload(tender, truck));
    }

    protected override IEnumerable<IComponent> GetRowOfFirstTable(int num, TransportationTender tender)
    {
        return TenderTableUtil.GetTableRow(num, tender);
    }

    protected override IEnumerable<IComponent> GetRowOfSecondTable(int num, Truck truck)
    {
        return TruckTableUtil.GetTableRow(num, truck);
    }
}