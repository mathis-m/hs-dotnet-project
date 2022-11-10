using System.Collections;

namespace UconsoleI.Elements;

public sealed class ElementCollectionIterator : IEnumerator<Element>
{
    private readonly List<ElementCollection> _lines;
    private          int                     _currentIndex;
    private          int                     _currentLine;
    private          bool                    _lineBreakEmitted;

    public ElementCollectionIterator(IEnumerable<ElementCollection> lines)
    {
        if (lines is null) throw new ArgumentNullException(nameof(lines));

        _currentLine  = 0;
        _currentIndex = -1;
        _lines        = new List<ElementCollection>(lines);

        Current = DefaultElements.Empty;
    }

    public Element Current { get; private set; }

    object? IEnumerator.Current => Current;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        if (_currentLine > _lines.Count - 1) return false;

        _currentIndex++;

        if (_currentIndex > _lines[_currentLine].Count - 1)
        {
            if (!_lineBreakEmitted)
                if (_currentIndex + 1 > _lines[_currentLine].Count - 1)
                    if (_currentLine + 1 <= _lines.Count - 1
                        && _lines[_currentLine + 1].Count > 0
                        && !_lines[_currentLine + 1][0].IsNewLine)
                    {
                        _lineBreakEmitted = true;
                        Current           = DefaultElements.LineBreak;
                        return true;
                    }

            _currentLine++;
            _currentIndex = 0;

            _lineBreakEmitted = false;

            var hasNoLinesLeft = _currentLine > _lines.Count - 1;
            if (hasNoLinesLeft) return false;

            while (_currentIndex > _lines[_currentLine].Count - 1)
            {
                _currentLine++;
                _currentIndex = 0;

                if (_currentLine > _lines.Count - 1) return false;
            }
        }

        _lineBreakEmitted = false;

        Current = _lines[_currentLine][_currentIndex];
        return true;
    }

    public void Reset()
    {
        _currentLine  = 0;
        _currentIndex = -1;

        Current = DefaultElements.Empty;
    }
}