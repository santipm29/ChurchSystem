using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChurchSystem.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChurchSystem.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : Controller
    {
        private readonly DataContext _context;
        public MeetingsController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetMeetings()
        {
            return Ok(_context.Meetings);
        }
    }
}
