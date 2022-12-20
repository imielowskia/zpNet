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
    public class CoursesController : Controller
    {
        private readonly zpnetContext _context;

        public CoursesController(zpnetContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
              return _context.Course != null ? 
                          View(await _context.Course.Include(c=>c.Students).ThenInclude(s=>s.Grades)
                            .Include(c=>c.Students).ThenInclude(s=>s.GradeDetails).ToListAsync()) :
                          Problem("Entity set 'zpnetContext.Course'  is null.");
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.Include(c=>c.Students).ThenInclude(s=>s.Grades)
                .Include(c=>c.Students).ThenInclude(s=>s.GradeDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'zpnetContext.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Metoda do formularza wystawiania ocen końcowych
        public async Task<IActionResult> TakeGrades(int id)
        {
            var course = await _context.Course.SingleAsync(c=>c.Id==id);
            var lista = new List<CG>();
            var xstud = _context.Student.Include(s=>s.Courses);
            foreach(var s in xstud)
            {
                if(s.Courses.Contains(course))
                {
                    lista.Add(new CG
                    {
                        StudentId = s.Id,
                        IN  = s.IN,
                        Grade = 0
                    }
                );
                }                
            }
            foreach(var s in lista)
            {
                var xtemp = _context.Grade.Where(g=>g.CourseId==id & g.StudentId==s.StudentId);
                if(xtemp.Count()>0 )
                {
                    var xg = _context.Grade.Where(g=>g.CourseId==course.Id & g.StudentId==s.StudentId).First();
                    if(xg != null){s.Grade=xg.Ocena;}
                }                
            }
            ViewData["listaOcen"]=lista;
            return View(course);
        }

        //Metoda do zapisywania ocen końcowych
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveGrades(int id)
        {
            var course = await _context.Course.SingleAsync(c=>c.Id==id);
            var xstudents = HttpContext.Request.Form["listaStudentow"];
            var xgrades = HttpContext.Request.Form["ListaOcen"];
            var ile = xstudents.Count();
            for(int i=0;i<ile;i++)
            {
                var xsid = int.Parse(xstudents[i]);
                var xgr = decimal.Parse(xgrades[i]);
                var xgrade = _context.Grade.Where(g=>g.CourseId==id & g.StudentId==xsid);
                if (xgrade.Any())
                {
                    var xocena = _context.Grade.Where(g=>g.CourseId==id & g.StudentId==xsid).Single();
                    xocena.Ocena=xgr;
                    _context.Update(xocena);
                }
                else
                {
                    var xocena = new Grade();
                    xocena.CourseId=id;
                    xocena.StudentId=xsid;
                    xocena.Ocena=xgr;
                    _context.Add(xocena);
                }                
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Metoda do formularza ocen bieżących
        public async Task<IActionResult> TakeGD (int id, int sid)
        {
            var course = await _context.Course.SingleAsync(c=>c.Id==id);
            var student = await _context.Student.Include(s=>s.GradeDetails).SingleAsync(s=>s.Id==sid);
            var xgrades = student.GradeDetails.Where(g=>g.CourseId==id);
            var xoceny = new List<GradeDetail>();
            if (xgrades.Count()>0)
            {
                foreach(var g in xgrades)
                {
                    xoceny.Add( new GradeDetail
                    {
                        Id=g.Id,
                        StudentId=sid,
                        CourseId=id,
                        Data = g.Data ,
                        Ocena = g.Ocena
                    }
                    );
                }
            }
            xoceny.Add( new GradeDetail
                    {
                        StudentId=sid,
                        CourseId=id,
                        Data = DateTime.Now.Date ,
                        Ocena = 0
                    }
                    );
            ViewData["listaOcen"]=xoceny;
            ViewData["IN"]=student.IN;
            ViewData["StudentId"]=student.Id;
            return View(course);
        }

        //metoda zapisywania ocen bieżących
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveGD(int id)
        {
            var course = await _context.Course.SingleAsync(c=>c.Id==id);
            var studentid = int.Parse(HttpContext.Request.Form["StudentId"]);
            var xgrades = _context.GradeDetail.Where(g=>g.CourseId==id & g.StudentId==studentid);
            var IdOcen = HttpContext.Request.Form["idOcen"];
            var oceny = HttpContext.Request.Form["ListaOcen"];
            var ile = oceny.Count();
            if(xgrades.Any())
            {
                for(int i=0;i<ile-1;i++)
                {
                    var gid = int.Parse(IdOcen[i]);
                    var xgd = _context.GradeDetail.Single(g=>g.Id==gid);
                    xgd.Ocena = decimal.Parse(oceny[i]);
                    if(xgd.Ocena>0)
                    {
                        _context.Update(xgd);
                    }
                    else
                    {
                        _context.Remove(xgd);
                    }
                }
            }
            if(decimal.Parse(oceny[ile-1])>0)
            {
                var ngd = new GradeDetail();
                ngd.CourseId = id;
                ngd.StudentId = studentid;
                ngd.Data = DateTime.Now.Date;
                ngd.Ocena = decimal.Parse(oceny[ile-1]);
                _context.Add(ngd);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Details",new {id=id});
        }
        private bool CourseExists(int id)
        {
          return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
