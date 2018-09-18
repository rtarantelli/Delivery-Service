using DeliveryService.Data.Interface;
using DeliveryService.Data.Model;
using System.Linq;

namespace DeliveryService.Data.Repository
{
    public class RouteRepository : Repository<Route>, IRouteRepository
    {
        public RouteRepository(DeliveryServiceContext context) : base(context) { }
    }
}
