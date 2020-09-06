using ChurchSystem.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChurchSystem.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class FieldsController: ControllerBase
    {
        private readonly DataContext _context;
        public FieldsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetFields()
        {
            return Ok(_context.Fields
                .Include(c => c.Districts)
                .ThenInclude(d => d.Churches));
        }

    }
}
