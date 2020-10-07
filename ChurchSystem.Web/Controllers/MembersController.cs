using ChurchSystem.Common.Enums;
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
    [Authorize(Roles = "Teacher")]
    public class MembersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        public MembersController(
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
            List<User> members = await _context.Users
                .Include(u => u.Church)
                .Include(u => u.Profession)
                .Where(u => u.Church.Id == loggedUser.Church.Id)
                .Where(u => u.UserType == UserType.User)
                .ToListAsync();
            return View(members);
        }

    }
}
