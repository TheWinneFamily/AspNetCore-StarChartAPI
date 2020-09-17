using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{

    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null) {
                return NotFound();
            }

            celestialObject.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id).ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {

            var celestialObjects = _context.CelestialObjects.Where(o => o.Name == name).ToList();

            if (!celestialObjects.Any())
            {
                return NotFound();
            }

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpPost()]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var foundCelestialObject = _context.CelestialObjects.Find(id);

            if (foundCelestialObject == null)
                return NotFound();

            foundCelestialObject.Name = celestialObject.Name;
            foundCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            foundCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.Update(foundCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var foundCelestialObject = _context.CelestialObjects.Find(id);

            if (foundCelestialObject == null)
                return NotFound();

            foundCelestialObject.Name = name;

            _context.Update(foundCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var foundCelestialObjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id).ToList();

            if (!foundCelestialObjects.Any())
                return NotFound();

            _context.RemoveRange(foundCelestialObjects);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
