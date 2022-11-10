using Common.Services;
using FreightMarket.Models;
using FreightMarket.Services;
using Microsoft.Extensions.DependencyInjection;
using VehicleAcquisition.Factories;
using VehicleAcquisition.Models;
using VehicleAcquisition.Services;


var services = new ServiceCollection();

// Register Randomizer
services
    .AddSingleton<IRandomizerService<Location>, LocationRandomizerService>()
    .AddSingleton<IRandomizerService<GoodTypes>, GoodTypeRandomizerService>()
    .AddSingleton<IRandomizerService<TransportationGoods>, TransportationGoodsRandomizerService>()
    .AddSingleton<IRandomizerService<TransportationTender>, TransportationTenderRandomizerService>();

// Register others
services
    .AddSingleton<ITenderPrinter, TenderPrinter>()
    .AddSingleton<IDeliveryCalculator, DeliveryCalculator>();


var serviceProvider = services.BuildServiceProvider();

var transportationTenderRandomizerService = serviceProvider.GetRequiredService<IRandomizerService<TransportationTender>>();
var tenderPrinter                         = serviceProvider.GetRequiredService<ITenderPrinter>();

var tenders = new List<TransportationTender>();
for (var i = 0; i < 10; i++)
{
    tenders.Add(await transportationTenderRandomizerService.NextAsync());
}

tenderPrinter.PrintTenders(tenders);