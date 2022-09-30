using Common.ConsoleUtils;
using CompanyNameDisplay;
using Microsoft.Extensions.DependencyInjection;

var sp = new ServiceCollection()
    .AddSingleton<IUserInput, UserInput>()
    .AddSingleton<ICompanyNameFormatter, StarAroundCompanyNameFormatter>()
    .BuildServiceProvider();

var formatter = sp.GetRequiredService<ICompanyNameFormatter>();
formatter.GatherNameAndPrint();