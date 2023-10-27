using Velozients.Models;
using Velozients.Services.Interfaces;

namespace Velozients.Services.Implementations
{
    public class PackageService : IPackageService
    {
        public PackageModel CreatePackages(string packageInformation, int packageId)
        {
            var packages = packageInformation.Split(',');
            
            PackageModel package = new();
            package.PackageName = packages[0].Trim();
            package.PackageName = package.PackageName.Substring(1, package.PackageName.Length - 2);

            string maxWeight = packages[1].Trim();
            package.PackageWeight = Convert.ToInt32(maxWeight.Substring(1, maxWeight.Length - 2));
            package.PackageId = packageId;

            return package;
        }
    }
}
