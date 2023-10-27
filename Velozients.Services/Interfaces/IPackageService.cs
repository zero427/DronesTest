using Velozients.Models;

namespace Velozients.Services.Interfaces
{
    public interface IPackageService
    {
        PackageModel CreatePackages(string packageInformation, int packageId);
    }
}
