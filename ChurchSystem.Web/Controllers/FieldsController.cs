using ChurchSystem.Common.Entities;
using ChurchSystem.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchSystem.Web.Controllers
{
    public class FieldsController : Controller
    {
        private readonly DataContext _context;

        public FieldsController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Fields
                .Include(f => f.Districts)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields
                .Include(f => f.Districts)
                .ThenInclude(d => d.Churches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (field == null)
            {
                return NotFound();
            }

            return View(field);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Field field)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    _context.Add(field);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(field);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }
            return View(field);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Field field)
        {
            if (id != field.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(field);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(field);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields
                .Include(f => f.Districts)
                .ThenInclude(d => d.Churches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (field == null)
            {
                return NotFound();
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool FieldExists(int id)
        {
            return _context.Fields.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddDistrict(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }

            District model = new District { IdField = field.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDistrict(District district)
        {
            if (ModelState.IsValid)
            {
                Field field = await _context.Fields
                    .Include(f => f.Districts)
                    .FirstOrDefaultAsync(f => f.Id == district.IdField);
                if (field == null)
                {
                    return NotFound();
                }

                try
                {
                    district.Id = 0;
                    field.Districts.Add(district);
                    _context.Update(field);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}",new {field.Id});

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(district);
        }

        public async Task<IActionResult> EditDistrict(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            District district = await _context.Districts.FindAsync(id);
            if (district == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields.FirstOrDefaultAsync(c => c.Districts.FirstOrDefault(d => d.Id == district.Id) != null);
            district.IdField = field.Id;
            return View(district);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDistrict(District district)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(district);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}/{ district.IdField}");

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(district);
        }

        public async Task<IActionResult> DeleteDistrict(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            District district = await _context.Districts
                .Include(d => d.Churches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields.FirstOrDefaultAsync(c => c.Districts.FirstOrDefault(d => d.Id == district.Id) != null);

            _context.Districts.Remove(district);
            await _context.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{field.Id}");
        }



    }
}
