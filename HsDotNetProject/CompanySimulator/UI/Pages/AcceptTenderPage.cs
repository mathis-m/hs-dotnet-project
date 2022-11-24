using CompanySimulator.State;
using CompanySimulator.State.Actions;
using FreightMarket.Models;
using UconsoleI.Components.TableComponent;
using UconsoleI.Components.TextComponent;
using UconsoleI.Extensions;
using UconsoleI.Rendering;

namespace CompanySimulator.UI.Pages;

public class AcceptTenderPage : BaseTablePage<TransportationTender>
{
    public AcceptTenderPage(StateManager stateManager) : base(stateManager)
    {
    }

    protected override string Prompt => "Accept a tender";

    protected override List<TableColumn> TableColumns => new()
    {
        new TableColumn(new Text("#").RightAligned()),
        new TableColumn(new Text("Goods")),
        new TableColumn(new Text("Type")),
        new TableColumn(new Text("Start location")),
        new TableColumn(new Text("Target location")),
        new TableColumn(new Text("Weight")),
        new TableColumn(new Text("Delivery Date")),
        new TableColumn(new Text("Compensation")),
        new TableColumn(new Text("Penalty")),
    };

    protected override IReadOnlyList<TransportationTender> GetSimulationList(RootState state)
    {
        return state.SimulationState.OpenTenders;
    }

    protected override ActionWithPayload<TransportationTender> GetOnSelectedAction(TransportationTender tender)
    {
        return new AcceptTenderAction(tender);
    }

    protected override IEnumerable<IComponent> GetTableRow(int num, TransportationTender tender)
    {
        return new List<IComponent>
        {
            new Text($"{num}")
                .RightAligned(),
            new Text($"{tender.TransportationGoods.Type}")
                .Overflow(Overflow.Wrap),
            new Text($"{tender.TransportationGoods.TruckType}")
                .Overflow(Overflow.Wrap),
            new Text(tender.StartLocation.ToString())
                .Overflow(Overflow.Wrap),
            new Text(tender.TargetLocation.ToString())
                .Overflow(Overflow.Wrap),
            new Text($"{tender.TransportationGoods.WeightInTons} T")
                .RightAligned()
                .Overflow(Overflow.Wrap),
            new Text($"{tender.DeliveryDate.ToShortDateString()}")
                .Overflow(Overflow.Wrap),
            new Text(tender.Compensation.FormattedValueWithIsoCode)
                .RightAligned()
                .Overflow(Overflow.Wrap),
            new Text(tender.Penalty.FormattedValueWithIsoCode)
                .RightAligned()
                .Overflow(Overflow.Wrap),
        };
    }
}