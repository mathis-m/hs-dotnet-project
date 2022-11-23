namespace UconsoleI.Internal;

internal sealed class DefaultPromptValue<T>
{
    public DefaultPromptValue(T value)
    {
        Value = value;
    }

    public T Value { get; }
}