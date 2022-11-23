using UconsoleI.Rendering;

namespace UconsoleI.UI;

public static class ConsoleUI
{
    private static Lazy<IConsoleUI> _console = new(
        () =>
        {
            var console = Create(new ConsoleUISettings(ColorSystemSupport.Detect, new ConsoleUiOutput(System.Console.Out)));


            Created = true;
            return console;
        });

    public static bool Created { get; set; }

    public static IConsoleUI Console
    {
        get => _console.Value;
        set
        {
            _console = new Lazy<IConsoleUI>(() => value);

            Created = true;
        }
    }


    public static IConsoleUI Create(ConsoleUISettings settings)
    {
        return ConsoleUIFactory.Create(settings);
    }


    public static void Write(IComponent component)
    {
        if (component is null) throw new ArgumentNullException(nameof(component));

        Console.Write(component);
    }
}