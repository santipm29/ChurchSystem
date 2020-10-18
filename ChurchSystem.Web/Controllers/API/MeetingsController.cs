using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ChurchSystem.Common.Enums;
using ChurchSystem.Common.Request;
using ChurchSystem.Common.Responses;
using ChurchSystem.Web.Data;
using ChurchSystem.Web.Data.Entities;
using ChurchSystem.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChurchSystem.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
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


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetMeetings()
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var meetings = _context.Meetings
                .Include(m => m.Assistances)
                .ThenInclude(m => m.User)
                .Where(m => m.Church == user.Church);

            return Ok(meetings);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("GetAssistances")]
        public async Task<IActionResult> GetAssistances()
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            var meetings = _context.Meetings
                .Include(m => m.Assistances)
                .Where(m => m.Church == user.Church);

            return Ok(meetings);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("Members")]
        public async Task<IActionResult> Members()
        {

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            List<User> members = await _context.Users
                .Include(u => u.Church)
                .Include(u => u.Profession)
                .Where(u => u.Church.Id == user.Church.Id)
                .Where(u => u.UserType == UserType.User)
                .ToListAsync();
        return Ok(members);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] MeetingRequest request)
        {

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            List<User> members = await _context.Users
                .Include(u => u.Church)
                .Include(u => u.Profession)
                .Where(u => u.Church.Id == user.Church.Id)
                .Where(u => u.UserType == UserType.User)
                .ToListAsync();

            var meeting = new Meeting
            {
                Church = user.Church,
                Date = request.Date
            };

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();


            foreach (var member in members)
            {
                _context.Assistances.Add(new Assistance
                {
                    Meeting = meeting,
                    User = member,
                    IsPresent = false
                });
            }
            await _context.SaveChangesAsync();
            return Ok(meeting);
        }
    }
}
