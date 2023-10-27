using Velozients.Models;

namespace Velozients.Strategies.PackageStrategy
{
    public class LightestFirstTopDownStrategy : BaseStrategy, IPackageStrategy
    {
        private List<DroneModel> Drones { get; set; }

        public LightestFirstTopDownStrategy(List<DroneModel> drones, List<PackageModel> packages) : base(packages) { Drones = drones; }

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
                    dronesPerTrip.Add(AssignPackagesToDrone(drone, GetPackagesBottomUpStrategy));
                }

                trip.Drones = dronesPerTrip;
                Trips.Add(trip);
            }

            return Trips;
        }

        // The first element is always the lightest package then you start arranging packages from heaviest to lightest in the same drom.
        private List<PackageModel> GetPackagesBottomUpStrategy(List<PackageModel> validPackagesForDrone, int remainingWeight, bool removePackage = true)
        {
            List<PackageModel> packages = new();

            // The lightest packet is taken and deleted from the list of pending packets.
            remainingWeight = CalculateRemainingWeight(packages, validPackagesForDrone[0], remainingWeight);

            if (remainingWeight == 0) return packages;

            // We try to fit as many packages as possible on the same drone, starting from the heaviest package to the lightest one.
            for (int i = validPackagesForDrone.Count - 1; i >= 1; i--)
            {
                if (remainingWeight - validPackagesForDrone[i].PackageWeight < 0) continue;

                remainingWeight = CalculateRemainingWeight(packages, validPackagesForDrone[i], remainingWeight);
            }

            return packages;
        }
    }
}
