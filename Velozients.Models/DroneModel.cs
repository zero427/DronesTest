namespace Velozients.Models
{
    public class DroneModel
    {
        public int DroneId { get; set; }
        public string DroneName { get; set; } = string.Empty;
        public int MaxWeight { get; set; }
        public List<PackageModel> Packages { get; set; }

        public DroneModel() => Packages = new List<PackageModel>();

        public DroneModel(DroneModel droneModel)
        {
            DroneId = droneModel.DroneId;
            DroneName = droneModel.DroneName;
            MaxWeight = droneModel.MaxWeight;
            Packages = new List<PackageModel>();
        }
    }
}
