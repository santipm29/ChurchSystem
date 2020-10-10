using ChurchSystem.Web.Data;
using ChurchSystem.Web.Data.Entities;
using ChurchSystem.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchSystem.Web.Controllers
{
    [Authorize(Roles = "Teacher, User")]
    public class MeetingsController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public MeetingsController(
            DataContext context,
            IUserHelper userHelper
        )
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task<IActionResult> Index()
        {
            User loggedUser = await _userHelper.GetUserAsync(User.Identity.Name);
            List<Meeting> meetings = await _context.Meetings
                .Include(m => m.Church)
                .Where(m => m.Church.Id == loggedUser.Church.Id)
                .ToListAsync();
            return View(meetings);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Meeting meeting = await _context.Meetings
                .Include(u => u.Church)
                .Include(a => a.Assistances)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }
    }
}
