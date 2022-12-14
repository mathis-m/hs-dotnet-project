namespace UconsoleI.Elements;

public class ElementCollection : List<Element>
{
    public ElementCollection()
    {
    }

    public ElementCollection(IEnumerable<Element> element)
        : base(element)
    {
    }

    public int Length => this.Sum(line => line.Text.Length);

    public void Prepend(Element element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));

        Insert(0, element);
    }
}