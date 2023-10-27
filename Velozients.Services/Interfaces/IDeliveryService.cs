using Microsoft.AspNetCore.Http;
using System.Text;

namespace Velozients.Services.Interfaces
{
    public interface IDeliveryService
    {   
        Task<StringBuilder> MakeDeliveriesByTrips(IFormFile file);
        Task<StringBuilder> MakeDeliveriesByDrones(IFormFile file);
    }
}
