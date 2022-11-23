using UconsoleI.Extensions;
using UconsoleI.Internal;

namespace UconsoleI.UI;

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
            profile,
            settings.ExclusivityMode ?? new DefaultExclusivityMode()
        );
    }
}