using UconsoleI.Rendering;

namespace UconsoleI.Extensions;

public static class AlignableExtensions
{
    public static T Alignment<T>(this T obj, Justify? alignment)
        where T : class, IAlignable
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        obj.Alignment = alignment;
        return obj;
    }

    public static T LeftAligned<T>(this T obj)
        where T : class, IAlignable
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        obj.Alignment = Justify.Left;
        return obj;
    }

    public static T Centered<T>(this T obj)
        where T : class, IAlignable
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        obj.Alignment = Justify.Center;
        return obj;
    }

    public static T RightAligned<T>(this T obj)
        where T : class, IAlignable
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        obj.Alignment = Justify.Right;
        return obj;
    }
}