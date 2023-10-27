using Velozients.Models;
using Velozients.Services.Interfaces;

namespace Velozients.Services.Implementations
{
    public class DroneService : IDroneService
    {
        public List<DroneModel> CreateDrones(string droneInformation)
        {
            List<DroneModel> result = new();
            var drones = droneInformation.Split(',');
            int droneId = 1;

            for(int i = 0; i<drones.Length; i = i+2)
            {
                DroneModel drone = new();
                drone.DroneName = drones[i].Trim();
                drone.DroneName = drone.DroneName.Substring(1, drone.DroneName.Length-2);

                string maxWeight = drones[i+1].Trim();
                drone.MaxWeight = Convert.ToInt32(maxWeight.Substring(1, maxWeight.Length - 2));
                drone.DroneId = droneId;
                droneId++;

                result.Add(drone);
            }

            return result;
        }
    }
}
