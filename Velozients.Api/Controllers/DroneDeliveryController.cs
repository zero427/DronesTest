using Microsoft.AspNetCore.Mvc;
using System.Text;
using Velozients.Services.Interfaces;

namespace Velozients.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DroneDeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DroneDeliveryController(IDeliveryService DeliveryService)
        {
            _deliveryService = DeliveryService;
        }

        [HttpPost("GenerateRouteMapByTrips")]
        public async Task<IActionResult> GenerateRouteMapByTrips(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                StringBuilder content = await _deliveryService.MakeDeliveriesByTrips(file);
                return Content(content.ToString(), "text/plain");
            }

            return BadRequest("No file was provided or the file is invalid.");
        }

        [HttpPost("GenerateRouteMapByDrones")]
        public async Task<IActionResult> GenerateRouteMapByDrones(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {

                StringBuilder content = await _deliveryService.MakeDeliveriesByDrones(file);
                return Content(content.ToString(), "text/plain");
            }

            return BadRequest("No file was provided or the file is invalid.");
        }
    }
}
