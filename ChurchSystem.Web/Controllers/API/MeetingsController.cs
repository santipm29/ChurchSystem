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
        [HttpGet]
        public IActionResult GetMeetings()
        {
            return Ok(_context.Meetings);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("Members")]
        public async Task<IActionResult> Members([FromBody] MemberRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            List<User> members = await _context.Users
                .Include(u => u.Church)
                .Include(u => u.Profession)
                .Where(u => u.Church.Id == request.ChurchId)
                .Where(u => u.UserType == UserType.User)
                .ToListAsync();
        return Ok(members);
        }
    }
}
