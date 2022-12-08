using CompanySimulator.State;
using CompanySimulator.State.Actions;
using UconsoleI.UI;

namespace CompanySimulator.UI.Pages;

public class CompanyNamePromptPage : IPage
{
    private readonly StateManager _stateManager;

    public CompanyNamePromptPage(StateManager stateManager)
    {
        _stateManager = stateManager;
    }

    public void Render()
    {
        var companyName = ConsoleUI.Ask<string>("What is your company name?");
        _stateManager.DispatchAction(new ChangeCompanyNameAction(companyName));
        _stateManager.DispatchAction(new DisplayPageAction(State.Pages.MainMenu));
    }

    public void Dispose()
    {
    }
}