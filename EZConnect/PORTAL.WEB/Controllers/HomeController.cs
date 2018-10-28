using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PORTAL.DAL.EF;
using PORTAL.DAL.EF.Helper;
using PORTAL.DAL.EF.Models;
using PORTAL.WEB.Models;
using PORTAL.WEB.Models.HomeViewModels;
using PORTAL.WEB.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PORTAL.WEB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserHandler _userHandler;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext context, IUserHandler userHandler, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userHandler = userHandler;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            //DashboardModel model = GenerateDashboardData();
            //return View(model);
            var user = string.Empty;
            user = _userManager.GetUserName(User);
            string hart = string.Empty;
            if(user == "hart17")
            {
                hart = Test();
            }

            return View((object)hart);
        }

        private string Test()
        {
            return "Test";
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
