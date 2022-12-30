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
    public class StudentsController : Controller
    {
        private readonly zpnetContext _context;

        public StudentsController(zpnetContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int id=0, string typ="")
        {
            var zpnetContext = _context.Student.Include(s => s.Field).Include(s=>s.Courses);
            ViewData["typ"]=typ;
            ViewData["id"]=id;
            return View(await zpnetContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Field)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Nazwa");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Indeks,Imie,Nazwisko,Data_u,FieldId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Nazwa", student.FieldId);
            return View(student);
            
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student.Include(s=>s.Field).Include(s=>s.Courses).SingleAsync(s=>s.Id== id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Nazwa", student.FieldId);
            GetCourseList(id);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Indeks,Imie,Nazwisko,Data_u,FieldId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    var xcourses = HttpContext.Request.Form["selectedCourses"];                    
                    var xsc = await _context.Student.Include(s=>s.Courses).SingleAsync(s=>s.Id==student.Id);
                    if(xsc.Courses != null) {xsc.Courses.Clear();} else {xsc.Courses = new List<Course>();};
                    foreach(var c in xcourses)
                    {
                        var xwyb = await _context.Course.SingleAsync(xc=>xc.Id==int.Parse(c));
                        xsc.Courses.Add(xwyb);
                    }
                    _context.Update(xsc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Nazwa", student.FieldId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Field)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Student == null)
            {
                return Problem("Entity set 'zpnetContext.Student'  is null.");
            }
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void GetCourseList(int? id=0)
        {
            var CoursesAll = _context.Course;
            var SelectedCourses = new List<CS>();
            var xst = _context.Student.Include(s=>s.Courses).Single(s=>s.Id == id);
            var xch = "";
            foreach(var c in CoursesAll)
            {
                
                if(xst.Courses.Contains(c)){xch="checked";} else {xch="";};
                SelectedCourses.Add(
                    new CS{
                        CourseId = c.Id,
                        Nazwa = c.Nazwa,
                        Checked = xch
                    }
                );
            }
            ViewData["courses"]=SelectedCourses;

        }

        private bool StudentExists(int id)
        {
          return (_context.Student?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
