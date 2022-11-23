using System.Text;
using UconsoleI.Components.ControlCodeComponent;
using UconsoleI.Elements;
using UconsoleI.Extensions;
using UconsoleI.Internal;
using UconsoleI.Rendering;
using UconsoleI.Stylings;
using UconsoleI.Stylings.Coloring;

namespace UconsoleI.UI;

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