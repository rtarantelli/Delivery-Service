using DeliveryService.Data.Interface;
using DeliveryService.Data.Model;

namespace DeliveryService.Data.Repository
{
    public class PointRepository : Repository<Point>, IPointRepository
    {
        public PointRepository(DeliveryServiceContext context) : base(context) { }
    }
}
