using DeliveryService.Data.Interface;
using DeliveryService.Data.Model;

namespace DeliveryService.Data.Repository
{
    public class PathRepository : Repository<Path>, IPathRepository
    {
        public PathRepository(DeliveryServiceContext context) : base(context) { }
    }
}
