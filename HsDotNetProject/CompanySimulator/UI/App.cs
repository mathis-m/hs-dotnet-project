using CompanySimulator.State;
using CompanySimulator.UI.MainMenu;
using Microsoft.Extensions.DependencyInjection;
using UconsoleI.UI;

namespace CompanySimulator.UI;

public class App : IStateListener
{
    private readonly Dictionary<Pages, Type> _pageMap = new()
    {
        { Pages.MainMenu, typeof(MainMenuPage) },
        { Pages.CompanyNamePrompter, typeof(CompanyNamePromptPage) },
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

    private void RenderPage(Pages page)
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