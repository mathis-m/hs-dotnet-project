namespace TruckDriver.Models;

public record DriverCategory(string Type)
{
    public const string DreamyType            = "Dreamy";
    public const string OldButExperiencedType = "Old, but experienced";
    public const string RacerType             = "Racer";
    public const string LovesHisJobType       = "Loves his job";
    public const string Inconspicuous         = "Inconspicuous";
}