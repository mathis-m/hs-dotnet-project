using CompanySimulator.State;
using CompanySimulator.UI.Pages;
using Microsoft.Extensions.DependencyInjection;
using UconsoleI.UI;

namespace CompanySimulator.UI;

public class App : IStateListener
{
    private readonly Dictionary<State.Pages, Type> _pageMap = new()
    {
        { State.Pages.MainMenu, typeof(MainMenuPage) },
        { State.Pages.CompanyNamePrompter, typeof(CompanyNamePromptPage) },
        { State.Pages.BuyTruck, typeof(BuyTruckPage) },
        { State.Pages.HireDriver, typeof(HireDriverPage) },
        { State.Pages.AcceptTender, typeof(AcceptTenderPage) },
    };

    private readonly IServiceProvider _serviceProvider;
    private readonly StateManager     _stateManager;

    public App(StateManager stateManager, IServiceProvider serviceProvider)
    {
        _stateManager    = stateManager;
        _serviceProvider = serviceProvider;
        stateManager.SubscribeToStateChanges(this);
    }

    public void OnStateChanged(RootState oldState, RootState newState)
    {
        var newPage = newState.ApplicationState.CurrentPage;
        if (oldState.ApplicationState.CurrentPage != newPage) RenderPage(newPage);
    }

    private void RenderPage(State.Pages page)
    {
        var pageType = _pageMap[page];
        if (pageType == null)
            throw new NotImplementedException($"{page} currently not supported");
        var pageImpl = _serviceProvider.GetRequiredService(pageType);
        if (pageImpl is not IPage validPage)
            throw new InvalidOperationException($"registered {page} must be IPage");
        ConsoleUI.Console.Clear();
        validPage.Render();
    }

    public void Execute()
    {
        RenderPage(_stateManager.CurrentState.ApplicationState.CurrentPage);
    }
}