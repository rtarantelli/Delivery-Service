using DeliveryService.Data.Interface;
using DeliveryService.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IPathRepository _pathRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly List<Route> _routes;

        private List<Route> Routes { get; set; } = new List<Route>();

        public RoutesController(IPathRepository pathRepository, IPointRepository pointRepository, IRouteRepository routeRepository)
        {
            _pathRepository = pathRepository;
            _pointRepository = pointRepository;
            _routeRepository = routeRepository;

            _routes = BuildRutes();
        }

        // GET: api/Routes/{origin}/{destiny}/{type}
        [HttpGet("{origin}/{destiny}/{type}")]
        public IActionResult GetRoutes([FromRoute] string origin, [FromRoute] string destiny, [FromRoute] char type)
        {
            var routes = _routes.FindAll(a => a.Path.Origin.Name == origin);

            foreach (var item in routes)
            {
                Routes.Add(item);

                NextRoute(item, destiny);
            }

            return Ok(Routes);
        }

        private List<Route> BuildRutes()
        {
            List<Route> routes = _routeRepository.GetAll().ToList();
            routes.ForEach(route =>
            {
                Path path = _pathRepository.GetById(route.PathId);
                path.Origin = _pointRepository.GetById(path.OriginId);
                path.Destiny = _pointRepository.GetById(path.DestinyId);

                route.Path = path;
            });

            return routes;
        }

        private Route NextRoute(Route route, string destiny)
        {
            var routes = _routes.FindAll(r => r.Path.OriginId == route.Path.DestinyId);

            foreach (var item in routes)
            {
                Routes.Add(item);

                if (item.Path.Destiny.Name == destiny)
                {
                    return null;
                }

                return NextRoute(item, destiny);
            }

            return null;
        }
    }
}