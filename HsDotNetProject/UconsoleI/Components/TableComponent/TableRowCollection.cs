using System.Collections;
using UconsoleI.Components.TextComponent;
using UconsoleI.Rendering;

namespace UconsoleI.Components.TableComponent;

public sealed class TableRowCollection : IReadOnlyList<TableRow>
{
    private readonly IList<TableRow> _list;
    private readonly object          _lock;
    private readonly Table           _table;

    internal TableRowCollection(Table table)
    {
        _table = table ?? throw new ArgumentNullException(nameof(table));
        _list  = new List<TableRow>();
        _lock  = new object();
    }

    TableRow IReadOnlyList<TableRow>.this[int index]
    {
        get
        {
            lock (_lock)
            {
                return _list[index];
            }
        }
    }

    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _list.Count;
            }
        }
    }

    public IEnumerator<TableRow> GetEnumerator()
    {
        lock (_lock)
        {
            var items = new TableRow[_list.Count];
            _list.CopyTo(items, 0);
            return new TableRowEnumerator(items);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Add(IEnumerable<IComponent> columns)
    {
        if (columns is null) throw new ArgumentNullException(nameof(columns));

        lock (_lock)
        {
            var row = CreateRow(columns);
            _list.Add(row);
            return _list.IndexOf(row);
        }
    }

    public int Insert(int index, IEnumerable<IComponent> columns)
    {
        if (columns is null) throw new ArgumentNullException(nameof(columns));

        lock (_lock)
        {
            var row = CreateRow(columns);
            _list.Insert(index, row);
            return _list.IndexOf(row);
        }
    }

    public void Update(int row, int column, IComponent cellData)
    {
        if (cellData is null) throw new ArgumentNullException(nameof(cellData));

        lock (_lock)
        {
            if (row < 0)
                throw new IndexOutOfRangeException("Table row index cannot be negative.");
            if (row >= _list.Count) throw new IndexOutOfRangeException("Table row index cannot exceed the number of rows in the table.");

            var tableRow = _list.ElementAtOrDefault(row);

            var currentRenderables = tableRow.ToList();

            if (column < 0)
                throw new IndexOutOfRangeException("Table column index cannot be negative.");
            if (column >= currentRenderables.Count) throw new IndexOutOfRangeException("Table column index cannot exceed the number of rows in the table.");

            currentRenderables.RemoveAt(column);

            currentRenderables.Insert(column, cellData);

            var newTableRow = new TableRow(currentRenderables);

            _list.RemoveAt(row);

            _list.Insert(row, newTableRow);
        }
    }

    public void RemoveAt(int index)
    {
        lock (_lock)
        {
            if (index < 0)
                throw new IndexOutOfRangeException("Table row index cannot be negative.");
            if (index >= _list.Count) throw new IndexOutOfRangeException("Table row index cannot exceed the number of rows in the table.");

            _list.RemoveAt(index);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _list.Clear();
        }
    }

    private TableRow CreateRow(IEnumerable<IComponent> columns)
    {
        var row = new TableRow(columns);

        if (row.Count > _table.Columns.Count) throw new InvalidOperationException("The number of row columns are greater than the number of table columns.");

        if (row.Count >= _table.Columns.Count) return row;

        var diff = _table.Columns.Count - row.Count;
        for (var i = 0; i < diff; i++) row.Add(Text.Empty);

        return row;
    }
}