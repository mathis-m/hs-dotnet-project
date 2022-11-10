using UconsoleI.Components.TextComponent;
using UconsoleI.Rendering;
using UconsoleI.Extensions;
using UconsoleI.Stylings;

namespace UconsoleI.Components.TableComponent;

public sealed class TableColumn : IColumn
{
    public IComponent Header { get; set; }
    public IComponent? Footer { get; set; }

    public Justify? Alignment { get; set; }
    public Padding? Padding { get; set; }
    public bool NoWrap { get; set; }
    public int? Width { get; set; }

    public TableColumn(string header, Justify? justification = null)
        : this(new Text(header).Overflow(Overflow.Ellipsis).Alignment(justification))
    {
    }

    public TableColumn(IComponent header)
    {
        Header    = header ?? throw new ArgumentNullException(nameof(header));
        Width     = null;
        Padding   = new Padding(1, 0, 1, 0);
        NoWrap    = false;
        Alignment = null;
        Footer    = null;
    }
}