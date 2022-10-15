using Common.ConsoleUtils;
using CompanyNameDisplay;
using Microsoft.Extensions.DependencyInjection;

var sp = new ServiceCollection()
    .AddSingleton<IUserInput<string>, StringUserInput>()
    .AddSingleton<ICompanyNameFormatter, StarAroundCompanyNameFormatter>()
    .BuildServiceProvider();

var userInput = sp.GetRequiredService<IUserInput<string>>();
var formatter = sp.GetRequiredService<ICompanyNameFormatter>();

var companyName = userInput.Prompt("Please enter the company name");
var formattedCompanyName = formatter.FormatCompanyName(companyName);
Console.WriteLine(formattedCompanyName);