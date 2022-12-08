using FreightMarket.Models;
using UconsoleI.Components.TableComponent;
using UconsoleI.Components.TextComponent;
using UconsoleI.Extensions;
using UconsoleI.Rendering;

namespace CompanySimulator.UI.Utils.TableUtils;

public static class TenderTableUtil
{
    public static List<TableColumn> TableColumns { get; } = new()
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

    public static IEnumerable<IComponent> GetTableRow(int num, TransportationTender tender)
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