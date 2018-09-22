using DeliveryService.Data.Interface;
using DeliveryService.Data.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DeliveryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PointsController : ControllerBase
    {
        private readonly IPointRepository _pointRepository;

        public PointsController(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        // GET: api/Points
        [HttpGet, AllowAnonymous]
        public IActionResult GetPoints()
        {
            try
            {
                return Ok(_pointRepository.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Points/5
        [HttpGet("{id}"), AllowAnonymous]
        public IActionResult GetPoint([FromRoute] int id)
        {
            try
            {
                Point point = _pointRepository.GetById(id);

                if (point == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(point);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Points/5
        [HttpPut("{id}")]
        public IActionResult PutPoint([FromRoute] int id, [FromBody] Point point)
        {
            if (id != point.PointId || _pointRepository.GetById(id) != null)
            {
                return BadRequest();
            }

            try
            {
                _pointRepository.Create(point);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Points
        [HttpPost]
        public IActionResult PostPoint([FromBody] Point point)
        {
            if (_pointRepository.Find(p => p.Name == point.Name).Any())
            {
                return BadRequest();
            }

            try
            {
                _pointRepository.Create(point);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Points/5
        [HttpDelete("{id}")]
        public IActionResult DeletePoint([FromRoute] int id)
        {
            Point point = _pointRepository.GetById(id);

            if (point == null)
            {
                return NotFound();
            }

            try
            {
                _pointRepository.Delete(point);
                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}