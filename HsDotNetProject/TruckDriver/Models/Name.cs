namespace TruckDriver.Models;

public record Name(string FirstName, string LastName)
{
    public string FullName => $"{FirstName} {LastName}";
}