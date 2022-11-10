using FreightMarket.Models;
using UconsoleI.Components.TableComponent;
using UconsoleI.Components.TableComponent.Borders;
using UconsoleI.Components.TextComponent;
using UconsoleI.Extensions;
using UconsoleI.Rendering;
using UconsoleI.UI;

namespace FreightMarket.Services;

public class TenderPrinter : ITenderPrinter
{
    public void PrintTenders(IList<TransportationTender> tenders)
    {
        var tenderTable = SetupTenderTable();
        for (var index = 0; index < tenders.Count; index++)
        {
            var transportationTender = tenders[index];
            tenderTable.AddRow(
                new Text($"{index + 1}").RightAligned(),
                new Text($"{transportationTender.TransportationGoods.Type}").Overflow(Overflow.Wrap),
                new Text($"{transportationTender.TransportationGoods.TruckType}").Overflow(Overflow.Wrap),
                new Text(transportationTender.StartLocation.ToString()).Overflow(Overflow.Wrap),
                new Text(transportationTender.TargetLocation.ToString()).Overflow(Overflow.Wrap),
                new Text($"{transportationTender.TransportationGoods.WeightInTons} T").Overflow(Overflow.Wrap),
                new Text($"{transportationTender.DeliveryDate.ToShortDateString()}").Overflow(Overflow.Wrap),
                new Text(transportationTender.Compensation.FormattedValueWithIsoCode).Overflow(Overflow.Wrap),
                new Text(transportationTender.Penalty.FormattedValueWithIsoCode).Overflow(Overflow.Wrap)
            );
        }


        ConsoleUI.Write(tenderTable);
    }

    private static Table SetupTenderTable()
    {
        var padding = new Padding(0, 1);

        var table = new Table
        {
            Border = TableBorderTypes.Square,
            Alignment = Justify.Center,
        };

        table.AddColumn(new TableColumn(new Text("#").RightAligned()).Padding(padding));
        table.AddColumn(new TableColumn(new Text("Goods")).Padding(padding));
        table.AddColumn(new TableColumn(new Text("Type")).Padding(padding));
        table.AddColumn(new TableColumn(new Text("Start location")).Padding(padding));
        table.AddColumn(new TableColumn(new Text("Target location")).Padding(padding));
        table.AddColumn(new TableColumn(new Text("Weight")).Padding(padding));
        table.AddColumn(new TableColumn(new Text("Delivery Date")).Padding(padding));
        table.AddColumn(new TableColumn(new Text("Compensation")).Padding(padding));
        table.AddColumn(new TableColumn(new Text("Penalty")).Padding(padding));

        return table;
    }
}