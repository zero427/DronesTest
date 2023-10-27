
namespace Velozients.Models
{
    public class TripModel
    {
        public int TripId { get; set; }
        public string TripName { get; set; } = string.Empty;
        public List<DroneModel> Drones { get; set; }

        public TripModel() => Drones = new List<DroneModel>();
    }
}
