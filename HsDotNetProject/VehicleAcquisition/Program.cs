using Common.Extensions;
using VehicleAcquisition.Models;
using VehicleAcquisition.Models.Trucks;

var esslingen = new Location("Esslingen");
var rom       = new Location("Rom");
var amsterdam = new Location("Amsterdam");
var istanbul  = new Location("Istanbul");

var trucks = new List<ITruck>
{
    new RefrigeratedTruck(Size.Small, 5, esslingen),
    new FlatbedTruck(Size.Medium, 7, esslingen),
    new FlatbedTruck(Size.Large, 0, rom),
    new TankerTruck(Size.Small, 1, amsterdam),
    new RefrigeratedTruck(Size.ExtraLarge, 3, istanbul),
};

trucks.Print();