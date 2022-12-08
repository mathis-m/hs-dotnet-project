using TruckDriver.Models;
using UconsoleI.Components.TableComponent;
using UconsoleI.Components.TextComponent;
using UconsoleI.Extensions;
using UconsoleI.Rendering;

namespace CompanySimulator.UI.Utils.TableUtils;

public static class DriverTableUtil
{
    public static List<TableColumn> TableColumns { get; } = new()
    {
        new TableColumn("#", Justify.Right),
        new TableColumn("Name", Justify.Center),
        new TableColumn("Salary", Justify.Center),
        new TableColumn("Type", Justify.Center),
    };

    public static IEnumerable<IComponent> GetTableRow(int num, TruckOperator driver)
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