using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChurchSystem.Common.Entities;
using ChurchSystem.Web.Data;

namespace ChurchSystem.Web.Controllers
{
    public class ChurchesController : Controller
    {
        private readonly DataContext _context;

        public ChurchesController(DataContext context)
        {
            _context = context;
        }

  
        public async Task<IActionResult> Index()
        {
            return View(await _context.Churches.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var church = await _context.Churches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (church == null)
            {
                return NotFound();
            }

            return View(church);
        }


        public IActionResult Create()
        {
            return View(new Church());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Church church)
        {
            if (ModelState.IsValid)
            {
                _context.Add(church);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(church);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var church = await _context.Churches.FindAsync(id);
            if (church == null)
            {
                return NotFound();
            }
            return View(church);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Church church)
        {
            if (id != church.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(church);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChurchExists(church.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(church);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var church = await _context.Churches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (church == null)
            {
                return NotFound();
            }

            _context.Churches.Remove(church);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ChurchExists(int id)
        {
            return _context.Churches.Any(e => e.Id == id);
        }
    }
}
