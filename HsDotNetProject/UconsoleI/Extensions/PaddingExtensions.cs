using UconsoleI.Rendering;

namespace UconsoleI.Extensions;

public static class PaddingExtensions
{
    public static int GetLeftSafe(this Padding? padding)
    {
        return padding?.Left ?? 0;
    }

    public static int GetRightSafe(this Padding? padding)
    {
        return padding?.Right ?? 0;
    }

    public static int GetTopSafe(this Padding? padding)
    {
        return padding?.Top ?? 0;
    }

    public static int GetBottomSafe(this Padding? padding)
    {
        return padding?.Bottom ?? 0;
    }
}