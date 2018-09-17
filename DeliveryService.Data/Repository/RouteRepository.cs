using DeliveryService.Data.Interface;
using DeliveryService.Data.Model;
using System.Linq;

namespace DeliveryService.Data.Repository
{
    public class RouteRepository : Repository<Route>, IRouteRepository
    {
        public RouteRepository(DeliveryServiceContext context) : base(context) { }

        public int GetTotalCost() =>
            _context.Routes?.Sum(r => r.Paths.Sum(p => p.Cost)) ?? 0;


        public int GetTotalTime() =>
            _context.Routes?.Sum(r => r.Paths.Sum(p => p.Time)) ?? 0;
    }
}
