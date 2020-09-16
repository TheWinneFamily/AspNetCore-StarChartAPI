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
    public class CelestialObjectController: ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}",Name="GetById")]
        public IActionResult GetById( int id )
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

            var celestialObjects = _context.CelestialObjects.Where<CelestialObject>(o => o.Name == name);

            if (celestialObjects.Count() == 0)
            {
                return NotFound();
            }

            foreach (var celestialObject in celestialObjects)
            {
                foreach (var orbitedObjects in _context.CelestialObjects.Where<CelestialObject>(o => o.OrbitedObjectId == celestialObject.Id))
                {
                    orbitedObjects.Satellites.Add(celestialObject);
                }
            }

            return Ok(celestialObjects);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;

            foreach( var celestialObject in celestialObjects)
            {
                foreach (var orbitedObjects in _context.CelestialObjects.Where<CelestialObject>(o => o.OrbitedObjectId == celestialObject.Id))
                {
                    orbitedObjects.Satellites.Add(celestialObject);
                }
            }

            return Ok(celestialObjects);
        }
    }
}
