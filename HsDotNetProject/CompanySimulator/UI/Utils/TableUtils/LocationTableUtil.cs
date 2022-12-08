using UconsoleI.Components.TableComponent;
using UconsoleI.Components.TextComponent;
using UconsoleI.Extensions;
using UconsoleI.Rendering;
using VehicleAcquisition.Models;

namespace CompanySimulator.UI.Utils.TableUtils;

public static class LocationTableUtil
{
    public static List<TableColumn> TableColumns { get; } = new()
    {
        new TableColumn("#", Justify.Right),
        new TableColumn("City", Justify.Center),
    };

    public static IEnumerable<IComponent> GetTableRow(int num, Location location)
    {
        return new List<IComponent>
        {
            new Text($"{num}")
                .RightAligned(),
            new Text($"{location.City}")
                .Overflow(Overflow.Wrap),
        };
    }
}