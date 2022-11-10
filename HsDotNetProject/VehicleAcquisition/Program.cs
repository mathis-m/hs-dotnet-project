using Common.Extensions;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;
using VehicleAcquisition.Services;

var esslingen = new Location("Esslingen");
var rom       = new Location("Rom");
var amsterdam = new Location("Amsterdam");
var istanbul  = new Location("Istanbul");

var trucks      = new List<ITruck>();
var rndmService = new TruckRandomizerService(new TruckAgeRandomizerService(), new LocationRandomizerService(), new TruckSizeRandomizerService());
for (var i = 0; i < 10; i++) trucks.Add(await rndmService.NextAsync());
trucks.Print();

Console.ReadLine();