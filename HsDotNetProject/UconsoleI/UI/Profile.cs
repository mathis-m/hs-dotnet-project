using System.Text;

namespace UconsoleI.UI;

public sealed class Profile
{
    private Encoding         _encoding;
    private int?             _height;
    private IConsoleUIOutput _out;
    private int?             _width;

    public Profile(IConsoleUIOutput @out, Encoding encoding)
    {
        _out      = @out ?? throw new ArgumentNullException(nameof(@out));
        _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
    }

    public IConsoleUIOutput Out
    {
        get => _out;
        set
        {
            _out = value ?? throw new InvalidOperationException("Output buffer cannot be null");

            if (!value.IsTerminal) return;

            _width  = null;
            _height = null;
        }
    }

    public Encoding Encoding
    {
        get => _encoding;
        set
        {
            if (value == null) throw new InvalidOperationException("Encoding cannot be null");

            _out.SetEncoding(value);
            _encoding = value;
        }
    }

    public int Width
    {
        get => _width ?? _out.Width;
        set
        {
            if (value <= 0) throw new InvalidOperationException("Console width must be greater than zero");

            _width = value;
        }
    }

    public int Height
    {
        get => _height ?? _out.Height;
        set
        {
            if (value <= 0) throw new InvalidOperationException("Console height must be greater than zero");

            _height = value;
        }
    }

    public ColorSystem ColorSystem { get; set; }

    public bool Interactive { get; set; }
}