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
            try
            {
                GetRoutesResult(origin, destiny, type);

                return Ok(Routes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void GetRoutesResult(string origin, string destiny, char type)
        {
            List<Route> routes = GetRoutesWithOriginAndDestiny(origin, destiny);

            List<Collection> collections = new List<Collection>();

            foreach (Route item in routes)
            {
                Routes.Add(item);

                GetNextRoute(item, destiny);

                if (Routes.Any(a => a.Path.Destiny.Name == destiny))
                {
                    collections.Add(new Collection() { Routes = Routes });
                }

                Routes = new List<Route>();
            }

            GetResultByType(type, collections);
        }

        private void GetResultByType(char type, List<Collection> collections)
        {
            if (type == 'C')
                Routes = collections.OrderBy(a => a.TotalCost()).FirstOrDefault().Routes;

            if (type == 'T')
                Routes = collections.OrderBy(a => a.TotalTime()).FirstOrDefault().Routes;

            if (type == 'S')
                Routes = collections.OrderBy(a => a.Routes.Count).FirstOrDefault().Routes;
        }

        private List<Route> GetRoutesWithOriginAndDestiny(string origin, string destiny) =>
            _routes.FindAll(a => a.Path.Origin.Name == origin).FindAll(a => a.Path.Destiny.Name != destiny);

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

        private Route GetNextRoute(Route route, string destiny)
        {
            List<Route> routes = _routes.FindAll(r => r.Path.OriginId == route.Path.DestinyId);

            foreach (Route item in routes)
            {
                Routes.Add(item);

                if (item.Path.Destiny.Name == destiny)
                {
                    return null;
                }

                return GetNextRoute(item, destiny);
            }

            return null;
        }
    }

    internal class Collection
    {
        public List<Route> Routes { get; set; }

        public int TotalCost() => Routes.Sum(a => a.Cost);

        public int TotalTime() => Routes.Sum(a => a.Time);
    }
}