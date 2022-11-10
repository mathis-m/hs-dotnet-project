using System.Runtime.InteropServices;
using System.Text;
using UconsoleI.Components.ControlCodeComponent;
using UconsoleI.Elements;
using UconsoleI.Extensions;
using UconsoleI.Internal;
using UconsoleI.Rendering;
using UconsoleI.Stylings;
using UconsoleI.Stylings.Coloring;

namespace UconsoleI.UI;

public enum CursorDirection
{
    Up,
    Down,
    Left,
    Right,
}

public interface IConsoleCursor
{
    void Show(bool show);
    void SetPosition(int column, int line);
    void Move(CursorDirection direction, int steps);
}

internal sealed class ConsoleUICursor : IConsoleCursor
{
    private readonly IConsoleUI _console;

    public ConsoleUICursor(IConsoleUI console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }

    public void Show(bool show)
    {
        _console.Write(show ? new ControlCode(AnsiSequences.SM(AnsiSequences.DECTCEM)) : new ControlCode(AnsiSequences.RM(AnsiSequences.DECTCEM)));
    }

    public void Move(CursorDirection direction, int steps)
    {
        if (steps == 0) return;

        switch (direction)
        {
            case CursorDirection.Up:
                _console.Write(new ControlCode(AnsiSequences.CUU(steps)));
                break;
            case CursorDirection.Down:
                _console.Write(new ControlCode(AnsiSequences.CUD(steps)));
                break;
            case CursorDirection.Right:
                _console.Write(new ControlCode(AnsiSequences.CUF(steps)));
                break;
            case CursorDirection.Left:
                _console.Write(new ControlCode(AnsiSequences.CUB(steps)));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public void SetPosition(int column, int line)
    {
        _console.Write(new ControlCode(AnsiSequences.CUP(line, column)));
    }
}

public interface IConsoleUI
{
    IConsoleCursor Cursor { get; }

    RenderPipeline Pipeline { get; }
    void Clear(bool home);

    void Write(IComponent component);
}

internal sealed class ConsoleUIFacade : IConsoleUI
{
    private readonly object _renderLock;

    public ConsoleUIFacade(Profile profile)
    {
        _renderLock = new object();

        Profile  = profile ?? throw new ArgumentNullException(nameof(profile));
        Input    = new DefaultInput(Profile);
        Pipeline = new RenderPipeline();
        Cursor   = new ConsoleUICursor(this);
    }

    public Profile Profile { get; }
    public IAnsiConsoleInput Input { get; }
    public IConsoleCursor Cursor { get; }
    public RenderPipeline Pipeline { get; }

    public void Clear(bool home)
    {
        lock (_renderLock)
        {
            Write(new ControlCode(AnsiSequences.ED(2)));
            Write(new ControlCode(AnsiSequences.ED(3)));

            if (home) Write(new ControlCode(AnsiSequences.CUP(1, 1)));
        }
    }

    public void Write(IComponent component)
    {
        lock (_renderLock)
        {
            var elements   = new List<Element>();
            var context    = new UIContext(Profile.ColorSystem);
            var components = Pipeline.Process(context, new[] { component });
            foreach (var comp in components) elements.AddRange(comp.Render(context, Profile.Width));

            var builder = new StringBuilder();
            foreach (var element in elements)
            {
                if (element.IsControlCode)
                {
                    builder.Append(element.Text);
                    continue;
                }

                var parts = element.Text.NormalizeNewLines().Split(new[] { '\n' });
                foreach (var (_, _, isLast, currentItem) in parts.EnumerateWithContext())
                {
                    if (!string.IsNullOrEmpty(currentItem)) builder.Append(BuildAnsiText(Profile, currentItem, element.Styling));

                    if (!isLast) builder.Append(Environment.NewLine);
                }
            }

            var text = builder.ToString();
            if (text.Length <= 0) return;

            Profile.Out.Writer.Write(text);
            Profile.Out.Writer.Flush();
        }
    }


    private static string BuildAnsiText(Profile profile, string text, Styling style)
    {
        if (style is null) throw new ArgumentNullException(nameof(style));

        var codes = AnsiDecorationBuilder.GetAnsiCodes(style.Decoration);

        if (style.Color != DefaultColors.Color)
            codes = codes.Concat(
                AnsiColorBuilder.GetAnsiCodes(
                    profile.ColorSystem,
                    style.Color,
                    true
                )
            );

        if (style.BackgroundColor != DefaultColors.BackgroundColor)
            codes = codes.Concat(
                AnsiColorBuilder.GetAnsiCodes(
                    profile.ColorSystem,
                    style.BackgroundColor,
                    false));

        var result = codes.ToArray();
        if (result.Length == 0) return text;

        var ansi = result.Length > 0
            ? $"{AnsiSequences.SGR(result)}{text}{AnsiSequences.SGR(0)}"
            : text;

        return ansi;
    }
}

internal sealed class DefaultInput : IAnsiConsoleInput
{
    private readonly Profile _profile;

    public DefaultInput(Profile profile)
    {
        _profile = profile ?? throw new ArgumentNullException(nameof(profile));
    }

    public bool IsKeyAvailable()
    {
        if (!_profile.Interactive) throw new InvalidOperationException("Failed to read input in non-interactive mode.");

        return Console.KeyAvailable;
    }

    public ConsoleKeyInfo? ReadKey(bool intercept)
    {
        if (!_profile.Interactive) throw new InvalidOperationException("Failed to read input in non-interactive mode.");

        return Console.ReadKey(intercept);
    }

    public async Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken)
    {
        if (!_profile.Interactive) throw new InvalidOperationException("Failed to read input in non-interactive mode.");

        while (true)
        {
            if (cancellationToken.IsCancellationRequested) return null;

            if (Console.KeyAvailable) break;

            await Task.Delay(5, cancellationToken).ConfigureAwait(false);
        }

        return ReadKey(intercept);
    }
}

public interface IAnsiConsoleInput
{
    bool IsKeyAvailable();
    ConsoleKeyInfo? ReadKey(bool intercept);
    Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken);
}

public static class ConsoleUI
{
    private static Lazy<IConsoleUI> _console = new(
        () =>
        {
            var console = Create(new ConsoleUISettings(ColorSystemSupport.Detect, new ConsoleUiOutput(System.Console.Out)));


            Created = true;
            return console;
        });

    public static bool Created { get; set; }

    public static IConsoleUI Console
    {
        get => _console.Value;
        set
        {
            _console = new Lazy<IConsoleUI>(() => value);

            Created = true;
        }
    }


    public static IConsoleUI Create(ConsoleUISettings settings)
    {
        return ConsoleUIFactory.Create(settings);
    }


    public static void Write(IComponent component)
    {
        if (component is null) throw new ArgumentNullException(nameof(component));

        Console.Write(component);
    }
}

public static class ConsoleUIFactory
{
    public static IConsoleUI Create(ConsoleUISettings settings)
    {
        if (settings is null) throw new ArgumentNullException(nameof(settings));

        var output = settings.Out ?? new ConsoleUiOutput(Console.Out);
        if (output.Writer == null) throw new InvalidOperationException("Output writer was null");

        var encoding = output.Writer.IsStandardOut() || output.Writer.IsStandardError()
            ? Console.OutputEncoding
            : output.Writer.Encoding;

        // Get the color system
        var colorSystem = settings.ColorSystem == ColorSystemSupport.Detect
            ? ColorSystemDetector.Detect()
            : (ColorSystem) settings.ColorSystem;


        var profile = new Profile(output, encoding)
        {
            ColorSystem = colorSystem,
            Interactive = settings.Interactive,
        };

        return new ConsoleUIFacade(
            profile
        );
    }
}

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

internal static class ColorSystemDetector
{
    public static ColorSystem Detect()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Windows 10.0.15063 and above support true color,
            if (!GetWindowsVersionInformation(out var major, out var build)) return ColorSystem.EightBit;
            switch (major)
            {
                case 10 when build >= 15063:
                case > 10:
                    return ColorSystem.TrueColor;
            }
        }
        else
        {
            return ColorSystem.TrueColor;
        }

        return ColorSystem.EightBit;
    }

    private static bool GetWindowsVersionInformation(out int major, out int build)
    {
        major = 0;
        build = 0;

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return false;

        var version = Environment.OSVersion.Version;
        major = version.Major;
        build = version.Build;
        return true;
    }
}

public sealed record ConsoleUISettings(ColorSystemSupport ColorSystem = ColorSystemSupport.Detect, IConsoleUIOutput Out = null, bool Interactive = true);

public sealed class ConsoleUiOutput : IConsoleUIOutput
{
    public ConsoleUiOutput(TextWriter writer)
    {
        Writer = writer ?? throw new ArgumentNullException(nameof(writer));
    }

    public TextWriter Writer { get; }

    public bool IsTerminal
    {
        get
        {
            if (Writer.IsStandardOut()) return !Console.IsOutputRedirected;

            if (Writer.IsStandardError()) return !Console.IsErrorRedirected;

            return false;
        }
    }

    public int Width => ConsoleHelper.GetSafeWidth();

    public int Height => ConsoleHelper.GetSafeHeight();

    public void SetEncoding(Encoding encoding)
    {
        if (Writer.IsStandardOut() || Writer.IsStandardError()) Console.OutputEncoding = encoding;
    }
}

public interface IConsoleUIOutput
{
    TextWriter Writer { get; }
    bool IsTerminal { get; }
    int Width { get; }
    int Height { get; }
    void SetEncoding(Encoding encoding);
}

public enum ColorSystem
{
    NoColors  = 0,
    Legacy    = 1,
    Standard  = 2,
    EightBit  = 3,
    TrueColor = 4,
}

public enum ColorSystemSupport
{
    Detect    = -1,
    NoColors  = 0,
    Legacy    = 1,
    Standard  = 2,
    EightBit  = 3,
    TrueColor = 4,
}