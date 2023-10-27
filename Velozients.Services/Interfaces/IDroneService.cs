using Velozients.Models;

namespace Velozients.Services.Interfaces
{
    public interface IDroneService
    {
        List<DroneModel> CreateDrones(string droneInformation);
    }
}
