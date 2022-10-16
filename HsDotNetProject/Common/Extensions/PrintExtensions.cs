namespace Common.Extensions;

public static class PrintExtensions
{
    public static void Print<TItem>(this IEnumerable<TItem> list, Func<TItem, string> selectTextDelegate,
        bool startAtZero = false)
    {
        var currentIdx = startAtZero ? 0 : 1;
        foreach (var item in list) Console.WriteLine($"{currentIdx++} {selectTextDelegate(item)}");
    }

    public static void Print<TItem>(this IEnumerable<TItem> list, bool startAtZero = false)
    {
        var currentIdx = startAtZero ? 0 : 1;
        foreach (var item in list) Console.WriteLine($"{currentIdx++} {item}");
    }
}