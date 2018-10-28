using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PORTAL.DAL.EF;
using PORTAL.DAL.EF.Models;

namespace PORTAL.WEB.Controllers
{
    public class OtpCodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OtpCodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OtpCodes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OtpCode.Include(o => o.Employee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OtpCodes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otpCode = await _context.OtpCode
                .Include(o => o.Employee)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (otpCode == null)
            {
                return NotFound();
            }

            return View(otpCode);
        }

        // GET: OtpCodes/Create
        public IActionResult Create()
        {
            ViewData["EmpId"] = new SelectList(_context.Employee, "Emp_ID", "Emp_ID");
            return View();
        }

        // POST: OtpCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,CreatedBy,ModifiedBy,CreatedOn,ModifiedOn,Status,EmpId")] OtpCode otpCode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(otpCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpId"] = new SelectList(_context.Employee, "Emp_ID", "Emp_ID", otpCode.EmpId);
            return View(otpCode);
        }

        // GET: OtpCodes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otpCode = await _context.OtpCode.SingleOrDefaultAsync(m => m.Id == id);
            if (otpCode == null)
            {
                return NotFound();
            }
            ViewData["EmpId"] = new SelectList(_context.Employee, "Emp_ID", "Emp_ID", otpCode.EmpId);
            return View(otpCode);
        }

        // POST: OtpCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Code,CreatedBy,ModifiedBy,CreatedOn,ModifiedOn,Status,EmpId")] OtpCode otpCode)
        {
            if (id != otpCode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(otpCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OtpCodeExists(otpCode.Id))
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
            ViewData["EmpId"] = new SelectList(_context.Employee, "Emp_ID", "Emp_ID", otpCode.EmpId);
            return View(otpCode);
        }

        // GET: OtpCodes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var otpCode = await _context.OtpCode
                .Include(o => o.Employee)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (otpCode == null)
            {
                return NotFound();
            }

            return View(otpCode);
        }

        // POST: OtpCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var otpCode = await _context.OtpCode.SingleOrDefaultAsync(m => m.Id == id);
            _context.OtpCode.Remove(otpCode);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OtpCodeExists(string id)
        {
            return _context.OtpCode.Any(e => e.Id == id);
        }

        public string validate(string code, string empId)
        {
            var otpCode = _context.OtpCode.Where(a => a.Code == code && a.EmpId == empId).SingleOrDefault();

            if (otpCode == null)
            {
                return "Error Code"; 
            }

            DateTime otpCodeTime = otpCode.ModifiedOn.Value.AddMinutes(5);
            DateTime currentDateTime = DateTime.Now;
            if (otpCodeTime < currentDateTime)
            {
                return "Expire Code";
            }
            return "Successfuly Validated";
        }
    }
}
