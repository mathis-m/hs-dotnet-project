using System.Collections;

namespace UconsoleI.Elements;

public class ElementCollectionEnumerator : IEnumerable<Element>
{
    private readonly List<ElementCollection> _lines;

    public ElementCollectionEnumerator(IEnumerable<ElementCollection> lines)
    {
        if (lines is null) throw new ArgumentNullException(nameof(lines));

        _lines = new List<ElementCollection>(lines);
    }

    public IEnumerator<Element> GetEnumerator()
    {
        return new ElementCollectionIterator(_lines);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}