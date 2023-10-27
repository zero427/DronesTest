using System.Text;
using Velozients.Models;
using Velozients.Services.Interfaces;

namespace Velozients.Services.Implementations
{
    public class TextUtilityService : ITextUtilityService
    {
        
        public StringBuilder GenerateSummary(List<TripModel> result, List<DroneModel> drones, bool showExtraInformation = false )
        {
            StringBuilder content = new StringBuilder();

            foreach (DroneModel localDrone in drones.OrderBy(r => r.DroneId))
            {
                var tipsSummaryByDron = result.Select(
                    x => new {
                        x.TripId,
                        DronePackages = x.Drones
                        .Where(z => z.DroneId == localDrone.DroneId && z.Packages.Count > 0)
                        .Select(y => y.Packages)
                    });

                content.AppendLine( (showExtraInformation) ? $"[{localDrone.DroneName} - Capacity:{localDrone.MaxWeight}]" : $"[{localDrone.DroneName}]");

                foreach (var dronSummary in tipsSummaryByDron)
                {

                    if (!dronSummary.DronePackages.Any()) continue;

                    content.AppendLine($"Trip #{dronSummary.TripId}");
                    string packagesSummary = string.Empty;
                    foreach (var package in dronSummary.DronePackages?.FirstOrDefault())
                    {
                        packagesSummary += (showExtraInformation) ? $"[{package.PackageName}: {package.PackageWeight}], ": $"[{package.PackageName}], ";
                    }

                    content.AppendLine(packagesSummary.Substring(0, packagesSummary.Length - 2));
                }

                content.AppendLine("");
            }

            return content;
        }
    }
}
