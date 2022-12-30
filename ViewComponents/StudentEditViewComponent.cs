using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using zpnet.Controllers;
using zpnet.Models;

namespace zpnet.ViewComponents;

public class StudentEditViewComponent:ViewComponent
{
    private readonly zpnetContext _context;
    public StudentEditViewComponent(zpnetContext context)
    {
        _context = context;
    }
    public async Task<IViewComponentResult> InvokeAsync(int id=0, string typ="pusty")
    {
        switch(typ)
        {
            case "nowy":
            {
                var xstudent = new Student{};
                ViewData["student"]=xstudent;
                ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Nazwa");
                return View("Create",xstudent);
            }
            case "edycja":
            {
                var student = await _context.Student.Include(s=>s.Field).Include(s=>s.Courses).SingleAsync(s=>s.Id== id);                
                ViewData["FieldId"] = new SelectList(_context.Field, "Id", "Nazwa", student.FieldId);
                GetCourseList(id);
                return View("Edit", student);                
            }
            default:
            {
                return View("Default");    
            }

        }
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
 
}