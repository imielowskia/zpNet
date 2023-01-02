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

public class DodatekViewComponent:ViewComponent
{
    private readonly zpnetContext _context;
    public DodatekViewComponent(zpnetContext context)
    {
        _context = context;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
       var liczba = _context.Student.Count();
       ViewData["ls"]=liczba;
        return View("Default"); 
    }
}