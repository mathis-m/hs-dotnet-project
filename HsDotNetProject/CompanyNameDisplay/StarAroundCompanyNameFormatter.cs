using Common.ConsoleUtils;

namespace CompanyNameDisplay;

internal class StarAroundCompanyNameFormatter : ICompanyNameFormatter
{
    private const string PromptText = "Please enter the company name";
    private readonly IUserInput _userInput;

    public StarAroundCompanyNameFormatter(IUserInput userInput)
    {
        _userInput = userInput;
    }

    public void GatherNameAndPrint()
    {
        var companyName = _userInput.PromptForString(PromptText);
        Console.WriteLine($"* {companyName} *");
    }
}