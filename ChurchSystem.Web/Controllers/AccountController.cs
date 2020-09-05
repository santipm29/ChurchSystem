﻿using ChurchSystem.Common.Entities;
using ChurchSystem.Common.Enums;
using ChurchSystem.Web.Data;
using ChurchSystem.Web.Data.Entities;
using ChurchSystem.Web.Helpers;
using ChurchSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IUserHelper _userHelper;

        public AccountController(
            DataContext context,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IBlobHelper blobHelper
        )
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;

        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email or password incorrect.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
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

                User user = await _userHelper.AddUserAsync(model, imageId, UserType.User);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    model.Fields = _combosHelper.GetComboFields();
                    model.Districts = _combosHelper.GetComboDistricts(model.FieldId);
                    model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
                    model.Professions = _combosHelper.GetComboProfessions();
                    return View(model);
                }

                LoginViewModel loginViewModel = new LoginViewModel
                {
                    Password = model.Password,
                    RememberMe = false,
                    Username = model.Username
                };

                Microsoft.AspNetCore.Identity.SignInResult result2 = await _userHelper.LoginAsync(loginViewModel);

                if (result2.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            model.Fields = _combosHelper.GetComboFields();
            model.Districts = _combosHelper.GetComboDistricts(model.FieldId);
            model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
            model.Professions = _combosHelper.GetComboProfessions();
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

        public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            District district = await _context.Districts.FirstOrDefaultAsync(d => d.Churches.FirstOrDefault(c => c.Id == user.Church.Id) != null);
            if (district == null)
            {
                district = await _context.Districts.FirstOrDefaultAsync();
            }

            Field field = await _context.Fields.FirstOrDefaultAsync(c => c.Districts.FirstOrDefault(d => d.Id == district.Id) != null);
            if (field == null)
            {
                field = await _context.Fields.FirstOrDefaultAsync();
            }
 
            EditUserViewModel model = new EditUserViewModel
            {
                Address = user.Address,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,
                Churches = _combosHelper.GetComboChurches(district.Id),
                Professions = _combosHelper.GetComboProfessions(),
                FieldId = field.Id,
                DistrictId = district.Id,
                ChurchId = user.Church.Id,
                Fields = _combosHelper.GetComboFields(),
                Districts = _combosHelper.GetComboDistricts(field.Id),
                Id = user.Id,
                Document = user.Document
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.ImageId = imageId;
                user.Profession = await _context.Professions.FindAsync(model.ProfessionId);
                user.Church = await _context.Churches.FindAsync(model.ChurchId);
                user.Profession = await _context.Professions.FindAsync(model.ProfessionId);
                user.Document = model.Document;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            model.Fields = _combosHelper.GetComboFields();
            model.Districts = _combosHelper.GetComboDistricts(model.FieldId);
            model.Churches = _combosHelper.GetComboChurches(model.DistrictId);
            model.Professions = _combosHelper.GetComboProfessions();
            return View(model);
        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }



    }

}

