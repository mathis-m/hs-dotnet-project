using System.Collections;
using UconsoleI.Rendering;

namespace UconsoleI.Components.TableComponent;

public sealed class TableRow : IEnumerable<IComponent>
{
    private readonly List<IComponent> _items;

    public TableRow(IEnumerable<IComponent>? items)
        : this(items, false, false)
    {
    }

    private TableRow(IEnumerable<IComponent>? items, bool isHeader, bool isFooter)
    {
        _items = new List<IComponent>(items ?? Array.Empty<IComponent>());

        IsHeader = isHeader;
        IsFooter = isFooter;
    }

    public int Count => _items.Count;

    internal bool IsHeader { get; }
    internal bool IsFooter { get; }

    public IComponent this[int index] => _items[index];

    public IEnumerator<IComponent> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal static TableRow Header(IEnumerable<IComponent> items)
    {
        return new TableRow(items, true, false);
    }

    internal static TableRow Footer(IEnumerable<IComponent> items)
    {
        return new TableRow(items, false, true);
    }

    internal void Add(IComponent item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        _items.Add(item);
    }
}