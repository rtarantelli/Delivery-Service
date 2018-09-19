using DeliveryService.Data.Interface;
using DeliveryService.Data.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PathsController : ControllerBase
    {
        private readonly IPathRepository _pathRepository;
        private readonly IPointRepository _pointRepository;

        public PathsController(IPathRepository pathRepository, IPointRepository pointRepository)
        {
            _pathRepository = pathRepository;
            _pointRepository = pointRepository;
        }

        // GET: api/Paths
        [HttpGet, AllowAnonymous]
        public IActionResult GetPaths()
        {
            try
            {
                List<Path> paths = _pathRepository.GetAll().ToList();

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
        [HttpGet("{id}"), AllowAnonymous]
        public IActionResult GetPath([FromRoute] int id)
        {
            try
            {
                Path path = _pathRepository.GetById(id);

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
            if (id != path.PathId || _pathRepository.GetById(id) != null)
            {
                return BadRequest();
            }

            try
            {
                _pathRepository.Create(path);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Paths
        [HttpPost]
        public IActionResult PostPath([FromBody] Path path)
        {
            if (_pathRepository.Find(p => p == path).Any())
            {
                return BadRequest();
            }

            try
            {
                _pathRepository.Create(path);
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
            Path path = _pathRepository.GetById(id);

            if (path == null)
            {
                return NotFound();
            }

            try
            {
                _pathRepository.Delete(path);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}