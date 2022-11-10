using Wcwidth;

namespace UconsoleI.Internal;

internal static class CellUtils
{
    private static readonly int?[] UnicodeCharCache = new int?[char.MaxValue];

    public static int GetCellLength(string text)
    {
        return text.Sum(GetCellLength);
    }

    public static int GetCellLength(char unicodeChar)
    {
        if (unicodeChar == '\n') return 1;

        return UnicodeCharCache[unicodeChar] ??= UnicodeCalculator.GetWidth(unicodeChar);
    }
}