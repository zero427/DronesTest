using Velozients.Models;

namespace Velozients.Strategies.PackageStrategy
{
    public interface IPackageStrategy
    {
        Task<List<TripModel>> CalculatePackagesByDrone();
    }
}
