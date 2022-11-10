using UconsoleI.UI;

namespace UconsoleI.Rendering;

public sealed record UIContext(ColorSystem ColorSystem, bool SingleLine = false, Justify? Justification = null);