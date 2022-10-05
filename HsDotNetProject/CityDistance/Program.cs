using CityDistance;
using Common.BngUtils;
using Common.ConsoleUtils;
using Microsoft.Extensions.DependencyInjection;

var sp = new ServiceCollection()
    .AddSingleton<IUserInput<string>, StringUserInput>()
    .AddSingleton<IUserInput<int>, IntUserInput>()
    .AddSingleton<IUserChoiceInput<int>, UserChoiceInput<int>>()
    .AddSingleton<IBngDistanceCalculator, BngDistanceCalculator>()
    .BuildServiceProvider();

var intChoiceInput = sp.GetRequiredService<IUserChoiceInput<int>>();
var distanceCalculator = sp.GetRequiredService<IBngDistanceCalculator>();

var choicePromptTexts = new List<string>
{
    "Enter start city",
    "Enter end city"
};

var cities = new City[]
{
    new("Amsterdam", new BngPoint(868851, 297477)),
    new("Berlin", new BngPoint(1442341, 404144)),
    new("Esslingen", new BngPoint(1232391, -71899)),
    new("Rom", new BngPoint(1605258, -786717)),
    new("Lissabon", new BngPoint(-220417, -1218006)),
    new("Istanbul", new BngPoint(3015490, -498084)),
    new("Aarhus", new BngPoint(1156381, 763352)),
    new("Tallinn", new BngPoint(1889074, 1368933))
};

var cityChoices = cities
    .Select((city, index) => new {city, key = index + 1})
    .ToDictionary(x => x.key, x => x.city.Name);


var selectedCityIndices = intChoiceInput.PromptUserMultiChoice(
    cityChoices,
    choicePromptTexts,
    "Choose 2 cities to calculate distance between them:"
);

var (startCity, startLocation) = cities[selectedCityIndices[0] - 1];
var (endCity, endLocation) = cities[selectedCityIndices[1] - 1];

var distance = distanceCalculator.CalculateDistanceInMeters(startLocation, endLocation);
var distanceInKm = Math.Round(distance / 1000);

Console.WriteLine($"\nThe distance between {startCity} and {endCity} is {distanceInKm} km.");

Console.ReadLine();