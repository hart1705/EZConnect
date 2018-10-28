using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PORTAL.DAL.EF;
using PORTAL.DAL.EF.Helper;
using PORTAL.DAL.EF.Models;


namespace PORTAL.WEB.Controllers.API
{
    [Produces("application/json")]

    public class EmpController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpController(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// added by Cedric
        /// 11/10/18
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("api/ez/employee/get_shares_line_manager/{empId}")]
        public IActionResult GetSharesLineManager(string empId)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = from s in _context.Employee
                           where (EF.Functions.Like(s.Line_Manager_No, empId + "%")) 
                           select s;
            if (employee == null)
            {
                return NotFound();
            }

            return new ObjectResult(employee);
        }

        [Route("api/ez/employee/get_direct_reports/{empId}")]
        public IActionResult GetDirectReports(string empId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = from s in _context.Employee
                           where (EF.Functions.Like(s.Line_Manager_No, empId + "%"))
                           select s;
            if (employee == null)
            {
                return NotFound();
            }

            return new ObjectResult(employee);
        }

        //[Route("api/Emp/{name}/{position}/{department}/{division}")]
        [Route("api/ez/employee/search_on_directory/{keyWord}")]
        public IActionResult GetEmployees(string keyWord)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = from s in _context.Employee
                           where (EF.Functions.Like(s.Employee_Name_English, "%" + keyWord + "%") 
                           || EF.Functions.Like(s.E_Mail, "%" + keyWord + "%")
                           || EF.Functions.Like(s.Position, "%" + keyWord + "%")
                           || EF.Functions.Like(s.Department, "%" + keyWord + "%")
                           || EF.Functions.Like(s.Division, "%" + keyWord + "%")
                           || EF.Functions.Like(s.Emp_ID, "%" + keyWord + "%")) 
                           //&& EF.Functions.Equals(s.Termination_Date).Equals(null)
                           select s;
            if (employee == null)
            {
                return NotFound();
            }

            return new ObjectResult(employee);
        }

        [Route("api/ez/employee/byname/{name}")]
        public IActionResult GetEmployee(string name)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = from s in _context.Employee
                           where EF.Functions.Like(s.Employee_Name_English, "%" + name + "%")
                           select s;
            if (employee == null)
            {
                return NotFound();
            }

            return new ObjectResult(employee);
        }

    }
}