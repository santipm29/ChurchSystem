using ChurchSystem.Common.Enums;
using ChurchSystem.Common.Responses;
using ChurchSystem.Web.Data;
using ChurchSystem.Web.Data.Entities;
using ChurchSystem.Web.Helpers;
using ChurchSystem.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeachersController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IUserHelper _userHelper;
        public TeachersController(
            DataContext context,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IBlobHelper blobHelper,
            IMailHelper mailHelper
        )
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;

        }
        public async Task<IActionResult> Index()
        {
            List<User> teachers = await _context.Users
                .Include(u => u.Church)
                .Include(u => u.Profession)
                .Where(u => u.UserType == UserType.Teacher)
                .ToListAsync();
            return View(teachers);
        }

        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Fields = _combosHelper.GetComboFields(),
                Districts = _combosHelper.GetComboDistricts(0),
                Churches = _combosHelper.GetComboChurches(0),
                Professions = _combosHelper.GetComboProfessions()
            };

            return View(model);
        }


        public JsonResult GetDistricts(int fieldId)
        {
            Field field = _context.Fields
                .Include(c => c.Districts)
                .FirstOrDefault(c => c.Id == fieldId);
            if (field == null)
            {
                return null;
            }

            return Json(field.Districts.OrderBy(d => d.Name));
        }

        public JsonResult GetChurches(int districtId)
        {
            District district = _context.Districts
                .Include(d => d.Churches)
                .FirstOrDefault(d => d.Id == districtId);
            if (district == null)
            {
                return null;
            }

            return Json(district.Churches.OrderBy(c => c.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.AddUserAsync(model, imageId, UserType.Teacher);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    model.Fields = _combosHelper.GetComboFields();
                    model.Districts = _combosHelper.GetComboDistricts(model.FieldId);
                    model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
                    model.Professions = _combosHelper.GetComboProfessions();
                    return View(model);
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);
                string password = model.Password;
                Response response = _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"Your password is, \"{password}\"" +
                    $"plase click in this link:<p><a href = \"{tokenLink}\">Confirm Email</a></p>");
                if (response.IsSuccess)
                {
                    ViewBag.Message = "The instructions to allow your user has been sent to email.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);

            }

            model.Fields = _combosHelper.GetComboFields();
            model.Districts = _combosHelper.GetComboDistricts(model.FieldId);
            model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
            model.Professions = _combosHelper.GetComboProfessions();
            return View(model);
        }




    }
}
