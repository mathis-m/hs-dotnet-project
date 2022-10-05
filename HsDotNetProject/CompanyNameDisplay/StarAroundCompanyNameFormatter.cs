using Common.ConsoleUtils;

namespace CompanyNameDisplay;

internal class StarAroundCompanyNameFormatter : ICompanyNameFormatter
{
    private const string PromptText = "Please enter the company name";
    private readonly IUserInput<string> _userInput;

    public StarAroundCompanyNameFormatter(IUserInput<string> userInput)
    {
        _userInput = userInput;
    }

    public void GatherNameAndPrint()
    {
        var companyName = _userInput.Prompt(PromptText);
        Console.WriteLine($"* {companyName} *");
    }
}