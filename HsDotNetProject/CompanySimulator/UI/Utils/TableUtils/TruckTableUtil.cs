using UconsoleI.Components.TableComponent;
using UconsoleI.Components.TextComponent;
using UconsoleI.Extensions;
using UconsoleI.Rendering;
using VehicleAcquisition.Models.Trucks;

namespace CompanySimulator.UI.Utils.TableUtils;

public static class TruckTableUtil
{
    public static List<TableColumn> TableColumns { get; } = new()
    {
        new TableColumn("#", Justify.Right),
        new TableColumn("Type", Justify.Center),
        new TableColumn("Age", Justify.Center),
        new TableColumn("Engine Power", Justify.Center),
        new TableColumn("Max Payload", Justify.Center),
        new TableColumn("Consumption", Justify.Center),
        new TableColumn("Price", Justify.Center),
        new TableColumn("Location", Justify.Center),
    };

    public static IEnumerable<IComponent> GetTableRow(int num, Truck truck)
    {
        return new List<IComponent>
        {
            new Text($"{num}")
                .RightAligned(),
            new Text($"{truck.TruckType}")
                .Overflow(Overflow.Wrap),
            new Text($"{truck.FormattedAge}")
                .RightAligned()
                .Overflow(Overflow.Wrap),
            new Text($"{truck.EnginePowerInKw} kW")
                .RightAligned()
                .Overflow(Overflow.Wrap),
            new Text($"{truck.MaxPayloadInTons} T")
                .RightAligned()
                .Overflow(Overflow.Wrap),
            new Text($"{truck.ConsumptionPer100KmInL} L")
                .RightAligned()
                .Overflow(Overflow.Wrap),
            new Text($"{truck.Price}")
                .RightAligned()
                .Overflow(Overflow.Wrap),
            new Text($"{truck.Location}")
                .Overflow(Overflow.Wrap),
        };
    }
}