using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryService.Data.Interface;
using DeliveryService.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IPathRepository _pathsRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IRouteRepository _routeRepository;

        public RoutesController(IPathRepository pathsRepository, IPointRepository pointRepository, IRouteRepository routeRepository)
        {
            _pathsRepository = pathsRepository;
            _pointRepository = pointRepository;
            _routeRepository = routeRepository;
        }

        // GET: api/Routes/{originId}/{destinyId}/{type}
        [HttpGet("{originId}/{destinyId}/{type}")]
        public IActionResult GetRoutes([FromRoute] int originId, [FromRoute] int destinyId, [FromRoute] char type)
        {

            return Ok();
        }
    }
}