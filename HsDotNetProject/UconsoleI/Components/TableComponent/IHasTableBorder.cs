using UconsoleI.Components.TableComponent.Borders;
using UconsoleI.Rendering;

namespace UconsoleI.Components.TableComponent;

public interface IHasTableBorder : IHasBorder
{
    public TableBorder Border { get; set; }
}