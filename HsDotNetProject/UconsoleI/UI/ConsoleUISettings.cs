namespace UconsoleI.UI;

public sealed record ConsoleUISettings(ColorSystemSupport ColorSystem = ColorSystemSupport.Detect, IConsoleUIOutput? Out = null, bool Interactive = true,
    IExclusivityMode? ExclusivityMode = null);