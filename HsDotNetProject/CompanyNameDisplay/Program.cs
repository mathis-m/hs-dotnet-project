using Common.ConsoleUtils;
using CompanyNameDisplay;
using Microsoft.Extensions.DependencyInjection;

var sp = new ServiceCollection()
    .AddSingleton<IUserInput<string>, StringUserInput>()
    .AddSingleton<ICompanyNameFormatter, StarAroundCompanyNameFormatter>()
    .BuildServiceProvider();

var formatter = sp.GetRequiredService<ICompanyNameFormatter>();
formatter.GatherNameAndPrint();