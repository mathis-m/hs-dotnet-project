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
        { State.Pages.AssignDriverToTruck, typeof(AssignDriverToTruckPage) },
        { State.Pages.RelocateTruck, typeof(RelocateTruckPage) },
        { State.Pages.AssignTenderToTuck, typeof(AssignTenderPage) },
    };

    private readonly IServiceProvider _serviceProvider;
    private readonly StateManager     _stateManager;

    public App(StateManager stateManager, IServiceProvider serviceProvider)
    {
        _stateManager    = stateManager;
        _serviceProvider = serviceProvider;
        stateManager.SubscribeToStateChanges(this);
    }

    private IPage? CurrentPage { get; set; }

    public void OnStateChanged(RootState oldState, RootState newState)
    {
        var newPage = newState.ApplicationState.CurrentPage;
        if (oldState.ApplicationState.CurrentPage != newPage) RenderPage(newPage);
    }

    private void RenderPage(State.Pages page)
    {
        CurrentPage?.Dispose();

        var pageType = _pageMap[page];
        if (pageType == null)
            throw new NotImplementedException($"{page} currently not supported");
        var pageImpl = _serviceProvider.GetRequiredService(pageType);
        if (pageImpl is not IPage validPage)
            throw new InvalidOperationException($"registered {page} must be IPage");

        CurrentPage = validPage;
        ConsoleUI.Console.Clear();
        validPage.Render();
        validPage.Dispose();
    }

    public void Execute()
    {
        RenderPage(_stateManager.CurrentState.ApplicationState.CurrentPage);
    }
}