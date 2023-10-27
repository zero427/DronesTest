using System;
using System.Collections.Generic;
using Velozients.Models;
using Velozients.Services.Implementations;
using Xunit;

namespace Velozients.TestServices
{
    public class DroneServiceTests
    {
        [Fact]
        public void CreateDrones_ValidInput_ReturnsListOfDrones()
        {
            // Arrange
            string droneInformation = "[Drone1], [10], [Drone2], [15]"; // Example of valid entry
            DroneService droneService = new DroneService();

            // Act
            List<DroneModel> drones = droneService.CreateDrones(droneInformation);

            // Assert
            Assert.NotNull(drones);
            Assert.Equal(2, drones.Count);

            // Check the specific values of the first drone
            Assert.Equal("Drone1", drones[0].DroneName);
            Assert.Equal(10, drones[0].MaxWeight);
            Assert.Equal(1, drones[0].DroneId);

            // Check the specific values of the second drone
            Assert.Equal("Drone2", drones[1].DroneName);
            Assert.Equal(15, drones[1].MaxWeight);
            Assert.Equal(2, drones[1].DroneId);
        }
    }
}
