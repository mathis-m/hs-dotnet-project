namespace UconsoleI.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<(int Index, bool IsFirst, bool IsLast, T Item)> EnumerateWithContext<T>(this IEnumerable<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return EnumerateWithContext(source.GetEnumerator());
    }

    public static IEnumerable<(int Index, bool IsFirst, bool IsLast, T Item)> EnumerateWithContext<T>(this IEnumerator<T> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        var first = true;
        var last  = !source.MoveNext();

        for (var index = 0; !last; index++)
        {
            var current = source.Current;
            last = !source.MoveNext();
            yield return (index, first, last, current);
            first = false;
        }
    }

    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> source, int count)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        var enumerable = source as T[] ?? source.ToArray();
        while (count-- > 0)
            foreach (var item in enumerable)
                yield return item;
    }

    public static int IndexOf<T>(this IEnumerable<T> source, T item)
        where T : class
    {
        var index = 0;
        foreach (var candidate in source)
        {
            if (candidate == item) return index;

            index++;
        }

        return -1;
    }
}