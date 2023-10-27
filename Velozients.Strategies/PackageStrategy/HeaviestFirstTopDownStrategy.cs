using Velozients.Models;

namespace Velozients.Strategies.PackageStrategy
{
    public class HeaviestFirstTopDownStrategy : BaseStrategy, IPackageStrategy
    {
        private List<DroneModel> Drones { get; set; }

        public HeaviestFirstTopDownStrategy(List<DroneModel> drones, List<PackageModel> packages) : base(packages) { Drones = drones; }

        public async Task<List<TripModel>> CalculatePackagesByDrone()
        {
            List<TripModel> Trips = new();
            Drones = Drones.OrderByDescending(x => x.MaxWeight).ToList();
            int tripId = 1;
            
            while (this.Packages.Any())
            {
                TripModel trip = new TripModel();
                trip.TripId = tripId++;
                List<DroneModel> dronesPerTrip = new List<DroneModel>();

                foreach (DroneModel drone in this.Drones)
                {
                    dronesPerTrip.Add(AssignPackagesToDrone(drone, GetPackagesTopDownStrategy));
                }

                trip.Drones = dronesPerTrip;
                Trips.Add(trip);
            }

            return Trips;
        }

        // The first element is always the heaviest package, then start arranging packages from the heaviest to the lightest in the same drom.
        private List<PackageModel> GetPackagesTopDownStrategy(List<PackageModel> validPackagesForDrone, int remainingWeight, bool removePackage = true)
        {
            List<PackageModel> packages = new();

            // The heaviest package is taken and deleted from the list of pending packages.
            remainingWeight = CalculateRemainingWeight(packages, validPackagesForDrone[validPackagesForDrone.Count - 1], remainingWeight);

            if (remainingWeight == 0) return packages;

            // We try to fit as many packages as possible on the same drone, starting from the heaviest package to the lightest one.
            for (int i = validPackagesForDrone.Count - 2; i >= 0; i--)
            {
                if (remainingWeight - validPackagesForDrone[i].PackageWeight < 0) continue;

                remainingWeight = CalculateRemainingWeight(packages, validPackagesForDrone[i], remainingWeight);
            }

            return packages;
        }
    }
}
