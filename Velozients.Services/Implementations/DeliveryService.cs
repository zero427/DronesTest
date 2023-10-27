using Microsoft.AspNetCore.Http;
using System.Text;
using Velozients.Models;
using Velozients.Models.Enums;
using Velozients.Services.Interfaces;
using Velozients.Strategies.PackageStrategy;

namespace Velozients.Services.Implementations
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDroneService _droneService;
        private readonly IPackageService _packageService;
        private readonly ITextUtilityService _textUtilityService;

        private List<DroneModel> Drones;
        private List<PackageModel> Packages;

        public DeliveryService(IDroneService droneService, IPackageService packageService, ITextUtilityService textUtilityService)
        {
            this._droneService = droneService;
            this._packageService = packageService;
            this._textUtilityService = textUtilityService;

            this.Drones = new();
            this.Packages = new();
        }

        public async Task<StringBuilder> MakeDeliveriesByDrones(IFormFile file)
        {
            await CreateDronesAndPackages(file);
            return await CreateTrips(PriorityEnum.ByDones);
        }

        public async Task<StringBuilder> MakeDeliveriesByTrips(IFormFile file)
        {
            await CreateDronesAndPackages(file);
            return await CreateTrips(PriorityEnum.ByTrips);
        }

        private async Task<StringBuilder> CreateTrips(PriorityEnum priority)
        {
            return _textUtilityService.GenerateSummary(priority==PriorityEnum.ByTrips ? await GetTripsPriorityTryps(): await GetTripsPriorityDrones(),this.Drones, (priority == PriorityEnum.ByDones));
        }

        private async Task<List<TripModel>> GetTripsPriorityTryps()
        {
            PackageStrategyContext PackageContext = new PackageStrategyContext(new TheBiggestBagStrategy(this.Drones, this.Packages));
            List<TripModel> result = await PackageContext.CalculatePackagesByDrone();

            return result;
        }

        private async Task<List<TripModel>> GetTripsPriorityDrones()
        {
            PackageStrategyContext heaviestFirstBottomUpStrategy = new PackageStrategyContext(new HeaviestFirstBottomUpStrategy(this.Drones, this.Packages.OrderBy(x=>x.PackageWeight).ToList()));
            PackageStrategyContext heaviestFirstTopDownStrategy = new PackageStrategyContext(new HeaviestFirstTopDownStrategy(this.Drones, this.Packages.OrderBy(x => x.PackageWeight).ToList()));
            PackageStrategyContext lightestFirstBottomUpStrategy = new PackageStrategyContext(new LightestFirstBottomUpStrategy(this.Drones, this.Packages.OrderBy(x => x.PackageWeight).ToList()));
            PackageStrategyContext lightestFirstTopDownStrategy = new PackageStrategyContext(new LightestFirstTopDownStrategy(this.Drones, this.Packages.OrderBy(x => x.PackageWeight).ToList()));

            Task<List<TripModel>> task1 = Task.Run(() => heaviestFirstBottomUpStrategy.CalculatePackagesByDrone());
            Task<List<TripModel>> task2 = Task.Run(() => heaviestFirstTopDownStrategy.CalculatePackagesByDrone());
            Task<List<TripModel>> task3 = Task.Run(() => lightestFirstBottomUpStrategy.CalculatePackagesByDrone());
            Task<List<TripModel>> task4 = Task.Run(() => lightestFirstTopDownStrategy.CalculatePackagesByDrone());

            // Wait for all tasks to complete
            var results = await Task.WhenAll(task1, task2, task3, task4);

            return results[GetBestStrategyIndex(results)];
        }

        private int GetBestStrategyIndex(List<TripModel>[] resultados)
        {
            List<StrategyResult> strategyResults = new();
            foreach (var (strategy, index) in resultados.Select((value, i) => (value, i)))
            {
                var trips = strategy.Select(x => new { drones = x.Drones.Where(z => z.Packages.Count > 0) }).Select(y => y.drones.Count()).Sum();
                strategyResults.Add(new StrategyResult { StrategyIndex = index, TotalTripsForAllDrones = trips });
            }

            return strategyResults.OrderBy(x=>x.TotalTripsForAllDrones).Select(z=>z.StrategyIndex).FirstOrDefault();
        }

        private async Task CreateDronesAndPackages(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                using (var reader = new StreamReader(stream))
                {
                    //read and create drones
                    this.Drones = _droneService.CreateDrones(await reader.ReadLineAsync());
                    int packageId = 0;
                    while (!reader.EndOfStream)
                    {
                        //read and create packages
                        PackageModel package = _packageService.CreatePackages(await reader.ReadLineAsync(), packageId++);
                        this.Packages.Add(package);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
