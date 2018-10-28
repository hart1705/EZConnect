using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PORTAL.DAL.EF;
using PORTAL.WEB.Controllers.API;
using PORTAL.WEB.Models.OtpLoginViewModels;

namespace PORTAL.WEB.Controllers
{
    public class OtpLoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public OtpLoginController(ApplicationDbContext context, ILogger<OtpLoginController> logger)
        {
            _context = context;
            _logger = logger;
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
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction(nameof(Validate));
                }
                else 
                {
                    ModelState.AddModelError(string.Empty, "User not found. " + results.ToString());
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Validate(string returnUrl = null)
        {
            //// Clear the existing external cookie to ensure a clean login process
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Validate(OtpCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                OTPController oTPController = new OTPController(_context);
                var result = oTPController.validate(model.Code);
                var results = Convert.ChangeType((result as ObjectResult)?.Value, typeof(string));
                if (results.ToString().Contains("Error Code"))
                {
                    ModelState.AddModelError(string.Empty, "Error Code.");
                    return View(model);
                }
                else if(results.ToString().Contains("Expired Codes"))
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
        public async Task<IActionResult> MyDirectory(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyDirectory(OtpCodeViewModel model)
        {
            
            return View(model);
        }
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByNameAsync(model.UserName);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //            return View(model);
        //        }
        //        else if (user.Status == Enums.Status.Inactive)
        //        {
        //            ModelState.AddModelError(string.Empty, "User not active. Kindly contact administrator.");
        //            return View(model);
        //        }
        //        else
        //        {
        //            // This doesn't count login failures towards account lockout
        //            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
        //            if (result.Succeeded)
        //            {
        //                _logger.LogInformation("User logged in.");
        //                return RedirectToLocal(returnUrl);
        //            }
        //            if (result.RequiresTwoFactor)
        //            {
        //                return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
        //            }
        //            if (result.IsLockedOut)
        //            {
        //                _logger.LogWarning("User account locked out.");
        //                return RedirectToAction(nameof(Lockout));
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //                return View(model);
        //            }
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}


    }
    //public static T GetValue<T>(this ActionResult<T> action)
    //{
    //    if (action.Value != null)
    //        return action.Value;
    //    else
    //        return (T)Convert.ChangeType((action.Result as ObjectResult)?.Value, typeof(T));
    //}
}