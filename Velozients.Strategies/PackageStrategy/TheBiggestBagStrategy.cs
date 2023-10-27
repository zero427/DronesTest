using Velozients.Models;

namespace Velozients.Strategies.PackageStrategy
{
    public class TheBiggestBagStrategy : BaseStrategy, IPackageStrategy
    {
        private List<DroneModel> Drones {  get; set; }

        public TheBiggestBagStrategy(List<DroneModel> drones, List<PackageModel> packages) : base(packages) { Drones = drones; }

        public async Task<List<TripModel>> CalculatePackagesByDrone()
        {
            List<TripModel> Trips = new();
            int tripId = 2;

            List<DroneModel> localDrones = Drones.OrderByDescending(x => x.MaxWeight).ToList();
            TripModel FirstTrip = CheckFirstTrip(localDrones);

            Trips.Add(FirstTrip);

            while (this.Packages.Any())
            {
                TripModel trip = new() { TripId = tripId++ };
                trip.Drones.Add(AssignPackagesToDrone(localDrones[0], GetPackagesTopDownStrategy));
                Trips.Add(trip);
            }

            return Trips; 
        }

        private TripModel CheckFirstTrip(List<DroneModel> localDrones)
        {
            TripModel result = new() { TripId = 1 };

            int remainingPackagesWeight = this.Packages.Sum(x => x.PackageWeight);
            double optimalNumberOfTrips = (double)remainingPackagesWeight / localDrones[0].MaxWeight;
            double upperLimit = Math.Ceiling(optimalNumberOfTrips);
            double lowerLimit = Math.Floor(optimalNumberOfTrips);

            if (lowerLimit < upperLimit)
            {
                // The value of the optimal number of trips rounded down is taken, then multiplied by the weight that the drone can carry.
                // that value is subtracted from the total weight and the result can be compared against the weight that the remaining drones can carry.
                foreach (DroneModel currentDrone in localDrones.Where(x => !x.DroneId.Equals(localDrones[0].DroneId)))
                {
                    DroneModel candidateDrone = new(currentDrone);
                    List<PackageModel> packages = new();

                    int missingWeight = remainingPackagesWeight - Convert.ToInt32(lowerLimit * localDrones[0].MaxWeight);
                    if (missingWeight > currentDrone.MaxWeight) break;

                    PackageModel firtsPackage = Packages.Where(x => x.PackageWeight == currentDrone.MaxWeight).FirstOrDefault();

                    if (firtsPackage != null)
                    {
                        packages.Add(firtsPackage);
                        candidateDrone.Packages = packages;

                        result.Drones.Add(candidateDrone);
                        this.Packages.Remove(firtsPackage);
                    }
                    else
                    {
                        // order the candidate packages from smallest to largest
                        List<PackageModel> validPackagesForDrone = Packages.Where(x => x.PackageWeight < currentDrone.MaxWeight).OrderBy(x => x.PackageWeight).ToList();
                        if (!validPackagesForDrone.Any()) break;

                        int remainingWeight = currentDrone.MaxWeight;

                        packages = GetPackagesTopDownStrategy(validPackagesForDrone, remainingWeight, false);
                        int weightLoadedOnTheDrone = packages.Sum(x => x.PackageWeight);

                        if (weightLoadedOnTheDrone < missingWeight) break;

                        foreach (PackageModel package in packages)
                        {
                            this.Packages.Remove(package);
                        }

                        candidateDrone.Packages = packages;
                        result.Drones.Add(candidateDrone);

                    }
                    // It should only run the process once, since the difference is only one trip in relation to the drone with the higher payload capacity.
                    break;
                }
            }

            result.Drones.Add(AssignPackagesToDrone(localDrones[0], GetPackagesTopDownStrategy));
            return result;
        }

        // The first element is always the heaviest package, then start arranging packages from the heaviest to the lightest in the same drom.
        private List<PackageModel> GetPackagesTopDownStrategy(List<PackageModel> validPackagesForDrone, int remainingWeight, bool removePackage = true)
        {
            List<PackageModel> packages = new();

            // The heaviest packet is taken and deleted from the list of pending packets.
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
