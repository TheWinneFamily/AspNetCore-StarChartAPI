using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{

    [Route("")]
    [ApiController]
    public class CelestialObjectController: ControllerBase
    {

        private ApplicationDbContext _context { get; }

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
