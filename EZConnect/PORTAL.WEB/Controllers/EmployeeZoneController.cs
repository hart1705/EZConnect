using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PORTAL.DAL.EF;
using PORTAL.DAL.EF.Models;
using PORTAL.WEB.Controllers.API;
using PORTAL.WEB.Models.EmployeeZoneViewModels;
using Microsoft.AspNetCore.Hosting;
namespace PORTAL.WEB.Controllers
{
    public class EmployeeZoneController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _env;

        public EmployeeZoneController(ApplicationDbContext context, ILogger<EmployeeZoneController> logger, IMapper mapper, IHostingEnvironment env)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(OtpLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                OTPController oTPController = new OTPController(_context);
                var result = await oTPController.PostEmployee(model.UserName);
                var results = Convert.ChangeType((result as ObjectResult)?.Value, typeof(string));
                if (results.ToString().Contains("Success"))
                {
                    var UserName = model.UserName;
                    var emailadd = await GetEmail(UserName);
                    _logger.LogInformation("User logged in.");
                    //ControllerContext.RouteData.Values.Add("email", emailadd);
                    //return RedirectToAction(nameof(Validate));

                    return RedirectToAction("Validate", "EmployeeZone", new { email = emailadd });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found. " + results.ToString());
                    return View(model);
                }
            }
            return View(model);
        }
        private async Task<string> GetEmail(string username)
        {
            var id = string.Empty;
            var employees = _context.Employee;
            foreach (var item in employees)
            {
                MailAddress addr = new MailAddress(item.E_Mail);
                string user = addr.User;
                string domain = addr.Host;
                if (user.ToLower() == username.ToLower())
                {
                    id = item.Emp_ID;
                    break;
                }

            }
            if (id == string.Empty)
            {
                return "Error";
            }
            var employeeRecord = _context.Employee.Where(a => a.Emp_ID == id).SingleOrDefault();
            
            return employeeRecord.E_Mail;
        }

        [HttpGet]
        public async Task<IActionResult> Validate(string email)
        {
            //// Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["email"] = email;
            
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Validate(OtpCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                OTPController oTPController = new OTPController(_context);
                //var code = model.Code1 + model.Code2 + model.Code3 + model.Code4 + model.Code5;
                var result = oTPController.validate(model.Code);
                var results = Convert.ChangeType((result as ObjectResult)?.Value, typeof(string));
                if (results.ToString().Contains("Error Code"))
                {
                    ModelState.AddModelError(string.Empty, "Error Code.");
                    return View(model);
                }
                else if (results.ToString().Contains("Expired Codes"))
                {
                    ModelState.AddModelError(string.Empty, "Expired Codes.");
                    return View(model);
                }
                else
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction(nameof(MyDirectory));
                    //return RedirectToAction(nameof(EmployeeZoneController.Index));
                }
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult MyDirectory(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
           // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult MyDirectory(OtpCodeViewModel model)
        {
            return RedirectToAction(nameof(Search));
            
        }
        public async Task<IActionResult> Search()
        {
            ViewData["webRoot"] = _env.WebRootPath;
            return View(await _context.Employee.OrderBy(a=>a.Hiring_Date).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(EmployeeDetailsViewModels model, string empId, string returnUrl = null)
        {
            
            ViewData["ReturnUrl"] = returnUrl;

            ViewData["empid"] = empId;
            var employeeRecord = _context.Employee.Where(a => a.Emp_ID == empId).SingleOrDefault();
            var LineManager = GetLineManager(employeeRecord.Line_Manager_No);
            var sharesLineManager = GetSharesLineManagerList(employeeRecord);
            var directReportsList = GetDirectReportsList(employeeRecord.Emp_ID);
            model = _mapper.Map<EmployeeDetailsViewModels>(employeeRecord);
            model.LineManager = LineManager.Result.Records;
            model.SharesLineManagers = sharesLineManager.Result.Records.OrderBy(a=>a.Hiring_Date);
            model.DirectReports = directReportsList.Result.Records.OrderBy(a => a.Hiring_Date);

            //return employeeRecord;
            return View(model);
        }
        private async Task<LineManagerList> GetLineManager(string lineManagerNo)
        {
            List<LineManagerModel> dbRec = new List<LineManagerModel>();
            var item = await _context.Employee.Where(a => a.Emp_ID == lineManagerNo).SingleOrDefaultAsync();

            var lineManagerModel = new LineManagerModel
            {
                Emp_ID = item.Emp_ID,
                Employee_Name_English = item.Employee_Name_English,
                Employee_Name_Arabic = item.Employee_Name_Arabic,
                Position = item.Position,
                E_Mail = item.E_Mail
            };

            dbRec.Add(lineManagerModel);


            LineManagerList lineManager = new LineManagerList
            {
                Records = dbRec
            };
            return lineManager;
        }
        private async Task<SharesLineManagerList> GetSharesLineManagerList(Employee employee)
        {
            List<SharesLineManagerModel> dbRec = new List<SharesLineManagerModel>();
            var record = await _context.Employee.Where(a => a.Line_Manager_No == employee.Line_Manager_No && a.Emp_ID != employee.Emp_ID).ToListAsync();
            foreach (var item in record)
            {
                var sharesLineManagerModel = new SharesLineManagerModel
                {
                    Emp_ID = item.Emp_ID,
                    Employee_Name_English = item.Employee_Name_English,
                    Employee_Name_Arabic = item.Employee_Name_Arabic,
                    Position = item.Position,
                    E_Mail = item.E_Mail,
                    Hiring_Date = item.Hiring_Date,
                    Grade = item.Grade
                };

                dbRec.Add(sharesLineManagerModel);

            }

            SharesLineManagerList sharesLineManagerList = new SharesLineManagerList
            {
                Records = dbRec
            };
            return sharesLineManagerList;
        }
        private async Task<DirectReportsList> GetDirectReportsList(string ManagerNo)
        {
            List<DirectReportsModel> dbRec = new List<DirectReportsModel>();
            var record = await _context.Employee.Where(a => a.Line_Manager_No == ManagerNo).ToListAsync();
            foreach (var item in record)
            {
                var directReportsModel = new DirectReportsModel
                {
                    Emp_ID = item.Emp_ID,
                    Employee_Name_English = item.Employee_Name_English,
                    Employee_Name_Arabic = item.Employee_Name_Arabic,
                    Position = item.Position,
                    E_Mail = item.E_Mail,
                    Hiring_Date = item.Hiring_Date,
                    Grade = item.Grade
                };

                dbRec.Add(directReportsModel);

            }

            DirectReportsList directReportsList = new DirectReportsList
            {
                Records = dbRec
            };
            return directReportsList;
        }


    }
}