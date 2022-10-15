namespace CompanyNameDisplay;

internal class StarAroundCompanyNameFormatter : ICompanyNameFormatter
{
    public string FormatCompanyName(string name)
    {
        return $"* {name} *";
    }
}