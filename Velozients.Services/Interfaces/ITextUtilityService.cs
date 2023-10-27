using System.Text;
using Velozients.Models;

namespace Velozients.Services.Interfaces
{
    public interface ITextUtilityService
    {
        StringBuilder GenerateSummary(List<TripModel> result, List<DroneModel> drones, bool showExtraInformation = false);
    }
}
