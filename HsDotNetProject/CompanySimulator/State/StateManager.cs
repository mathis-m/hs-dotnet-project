using CompanySimulator.Factories;
using CompanySimulator.State.Actions;
using CompanySimulator.State.Reducers;
using Microsoft.Extensions.DependencyInjection;

namespace CompanySimulator.State;

// not thread safe would need to lock state if concurrency is required
public class StateManager
{
    private readonly List<IStateListener> _listeners = new();
    private readonly IServiceProvider     _serviceProvider;

    public StateManager(IServiceProvider serviceProvider, IInitialStateFactory initialStateFactory)
    {
        _serviceProvider = serviceProvider;
        CurrentState     = initialStateFactory.CreateInitialState().Result;
    }

    public RootState CurrentState { get; private set; }

    public void DispatchAction<TPayload>(ActionWithPayload<TPayload> action)
    {
        var reducer = _serviceProvider.GetRequiredService(action.ReducerType);
        if (reducer is not IReducerT<TPayload> validReducer) throw new InvalidOperationException("The action must define a valid reducer type");

        var oldState = CurrentState;
        var newState = validReducer.Reduce(oldState, action.Payload);
        CurrentState = newState;
        NotifyListeners(oldState, newState);
    }


    public void DispatchAction(ActionWithoutPayload action)
    {
        var baseType = action.GetType().BaseType;
        if (baseType is { IsGenericType: true } && baseType.GetGenericTypeDefinition() == typeof(ActionWithPayload<>))
        {
            var genericOverload = typeof(StateManager).GetMethods().FirstOrDefault(x => x.IsGenericMethod && x.Name == "DispatchAction");
            if (genericOverload is null) return;

            var fooRef = genericOverload.MakeGenericMethod(baseType.GetGenericArguments()[0]);
            fooRef.Invoke(this, new object?[] { action });

            return;
        }

        var reducer = _serviceProvider.GetRequiredService(action.ReducerType);
        if (reducer is not IReducer validReducer) throw new InvalidOperationException("The action must define a valid reducer type");

        var oldState = CurrentState;
        var newState = validReducer.Reduce(oldState);
        CurrentState = newState;
        NotifyListeners(oldState, newState);
    }

    private void NotifyListeners(RootState oldState, RootState newState)
    {
        _listeners.ForEach(listener => listener.OnStateChanged(oldState, newState));
    }


    public void SubscribeToStateChanges(IStateListener listener)
    {
        _listeners.Add(listener);
    }


    public void RemoveListener(IStateListener listener)
    {
        try
        {
            _listeners.Remove(listener);
        }
        catch
        {
            // Ignore was already removed
        }
    }
}