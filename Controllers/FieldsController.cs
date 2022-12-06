using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using zpnet.Models;

namespace zpnet.Controllers
{
    public class FieldsController : Controller
    {
        private readonly zpnetContext _context;

        public FieldsController(zpnetContext context)
        {
            _context = context;
        }

        // GET: Fields
        public async Task<IActionResult> Index()
        {
              return _context.Field != null ? 
                          View(await _context.Field.ToListAsync()) :
                          Problem("Entity set 'zpnetContext.Field'  is null.");
        }

        // GET: Fields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Field == null)
            {
                return NotFound();
            }

            var @field = await _context.Field
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@field == null)
            {
                return NotFound();
            }

            return View(@field);
        }

        // GET: Fields/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Opis")] Field @field)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@field);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@field);
        }

        // GET: Fields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Field == null)
            {
                return NotFound();
            }

            var @field = await _context.Field.FindAsync(id);
            if (@field == null)
            {
                return NotFound();
            }
            return View(@field);
        }

        // POST: Fields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Opis")] Field @field)
        {
            if (id != @field.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@field);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FieldExists(@field.Id))
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
            return View(@field);
        }

        // GET: Fields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Field == null)
            {
                return NotFound();
            }

            var @field = await _context.Field
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@field == null)
            {
                return NotFound();
            }

            return View(@field);
        }

        // POST: Fields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Field == null)
            {
                return Problem("Entity set 'zpnetContext.Field'  is null.");
            }
            var @field = await _context.Field.FindAsync(id);
            if (@field != null)
            {
                _context.Field.Remove(@field);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FieldExists(int id)
        {
          return (_context.Field?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
