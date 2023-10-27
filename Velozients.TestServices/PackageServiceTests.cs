using Velozients.Models;
using Velozients.Services.Implementations;

namespace Velozients.TestServices
{
    public class PackageServiceTests
    {
        [Fact]
        public void CreatePackages_ValidInput_ReturnsPackageModel()
        {
            // Arrange
            string packageInformation = "[Package1], [10]";
            int packageId = 1;
            PackageService packageService = new PackageService();

            // Act
            PackageModel package = packageService.CreatePackages(packageInformation, packageId);

            // Assert
            Assert.NotNull(package);

            // Check package specific values
            Assert.Equal("Package1", package.PackageName);
            Assert.Equal(10, package.PackageWeight);
            Assert.Equal(1, package.PackageId);
        }
    }
}
