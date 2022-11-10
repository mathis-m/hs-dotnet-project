using UconsoleI.Rendering;

namespace UconsoleI.Extensions;

public static class OverflowableExtensions
{
    public static T Overflow<T>(this T obj, Overflow overflow)
        where T : class, IOverflowable
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Overflow = overflow;
        return obj;
    }
}