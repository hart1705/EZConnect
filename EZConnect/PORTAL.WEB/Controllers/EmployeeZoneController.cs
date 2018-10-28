using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PORTAL.DAL.EF;

namespace PORTAL.WEB.Controllers
{
    public class EmployeeZoneController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeZoneController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Search()
        {
            return View(await _context.Employee.ToListAsync());
        }
    }
}