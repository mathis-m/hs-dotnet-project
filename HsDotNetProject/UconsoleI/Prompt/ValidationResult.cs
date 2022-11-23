namespace UconsoleI.Prompt;

public sealed class ValidationResult
{
    private ValidationResult(bool successful, string? message)
    {
        Successful = successful;
        Message    = message;
    }

    public bool Successful { get; }
    public string? Message { get; }

    public static ValidationResult Success()
    {
        return new ValidationResult(true, null);
    }

    public static ValidationResult Error(string? message = null)
    {
        return new ValidationResult(false, message);
    }
}