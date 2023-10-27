using Velozients.Models;

namespace Velozients.Strategies.PackageStrategy
{
    public abstract class BaseStrategy
    {
        protected List<PackageModel> Packages { get; set; }

        public BaseStrategy(List<PackageModel> packages) => Packages = packages;

        // Statement of the delegate
        protected delegate List<PackageModel> PackagesStrategy(List<PackageModel> validPackagesForDrone, int remainingWeight, bool removePackage = true);

        protected virtual DroneModel AssignPackagesToDrone(DroneModel drone, PackagesStrategy packagesStrategy)
        {
            DroneModel result = new(drone);
            List<PackageModel> packages = new();

            PackageModel firtsPackage = Packages.Where(x => x.PackageWeight == drone.MaxWeight).FirstOrDefault();

            if (firtsPackage != null)
            {
                packages.Add(firtsPackage);
                result.Packages = packages;

                this.Packages.Remove(firtsPackage);
            }
            else
            {
                // Order the candidate packages from smallest to largest
                List<PackageModel> validPackagesForDrone = Packages.Where(x => x.PackageWeight < drone.MaxWeight).OrderBy(x => x.PackageWeight).ToList();
                if (!validPackagesForDrone.Any()) return result;

                int remainingWeight = drone.MaxWeight;

                packages = packagesStrategy(validPackagesForDrone, remainingWeight);

            }
            result.Packages = packages;
            return result;
        }

        protected int CalculateRemainingWeight(List<PackageModel> packages, PackageModel newPackage, int currentRemainingWeight, bool removePackage = true)
        {
            packages.Add(newPackage);
            if (removePackage) { this.Packages.Remove(newPackage); }
            return currentRemainingWeight - newPackage.PackageWeight;
        }

    }
}
