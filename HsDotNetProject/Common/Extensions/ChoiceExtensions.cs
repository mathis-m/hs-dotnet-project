namespace CityDistance;

public static class ChoiceExtensions
{
    public static Dictionary<int, string> ToChoiceDictionary<TItem>(this TItem[] array,
        Func<TItem, string> selectTextDelegate, bool startAtZero = false)
    {
        var offset = startAtZero ? 0 : 1;

        return array
            .Select((item, index) => new {text = selectTextDelegate(item), key = index + offset})
            .ToDictionary(x => x.key, x => x.text);
    }
}