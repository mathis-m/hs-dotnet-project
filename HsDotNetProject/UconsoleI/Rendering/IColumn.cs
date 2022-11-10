namespace UconsoleI.Rendering;

public interface IColumn : IAlignable, IPaddable
{
    bool NoWrap { get; set; }
    int? Width { get; set; }
}