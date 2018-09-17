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
    public class PathsController : ControllerBase
    {
        private readonly IPathRepository _pathsRepository;
        private readonly IPointRepository _pointRepository;

        public PathsController(IPathRepository pathsRepository, IPointRepository pointRepository)
        {
            _pathsRepository = pathsRepository;
            _pointRepository = pointRepository;
        }

        // GET: api/Paths
        [HttpGet]
        public IActionResult GetPaths()
        {
            try
            {
                List<Path> paths = _pathsRepository.GetAll().ToList();

                paths.ForEach(path =>
                {
                    path.Origin = _pointRepository.GetById(path.OriginId);
                    path.Destiny = _pointRepository.GetById(path.DestinyId);
                });

                return Ok(paths);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Paths/5
        [HttpGet("{id}")]
        public IActionResult GetPath([FromRoute] int id)
        {
            try
            {
                Path path = _pathsRepository.GetById(id);

                if (path == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(path);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Paths/5
        [HttpPut("{id}")]
        public IActionResult PutPath([FromRoute] int id, [FromBody] Path path)
        {
            if (id != path.PathId || _pathsRepository.GetById(id) != null)
            {
                return BadRequest();
            }

            try
            {
                _pathsRepository.Create(path);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Paths/5
        [HttpPost]
        public IActionResult PostPath([FromBody] Path path)
        {
            bool exists = _pathsRepository
                .Find(p => p.PathId == path.PathId || (p.Destiny.PointId == path.Destiny.PointId && p.Origin.PointId == path.Origin.PointId))
                .Any();

            if (exists)
            {
                return BadRequest();
            }

            try
            {
                _pathsRepository.Create(path);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Paths/5
        [HttpDelete("{id}")]
        public IActionResult DeletePath([FromRoute] int id)
        {
            Path path = _pathsRepository.GetById(id);

            if (path == null)
            {
                return NotFound();
            }

            try
            {
                _pathsRepository.Delete(path);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}