using Velozients.Models;

namespace Velozients.Strategies.PackageStrategy
{
    public class PackageStrategyContext
    {
        protected IPackageStrategy _packageStrategy;

        public PackageStrategyContext(IPackageStrategy packageStrategy)
        {
            this._packageStrategy = packageStrategy;
        }

        public async Task<List<TripModel>> CalculatePackagesByDrone()
        {
            return await this._packageStrategy.CalculatePackagesByDrone();
        }
    }
}
