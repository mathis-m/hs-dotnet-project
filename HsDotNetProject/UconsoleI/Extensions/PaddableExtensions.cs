using UconsoleI.Rendering;

namespace UconsoleI.Extensions;
public static class PaddableExtensions
{
    public static T Padding<T>(this T obj, Padding padding)
        where T : class, IPaddable
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        obj.Padding = padding;
        return obj;
    }
}