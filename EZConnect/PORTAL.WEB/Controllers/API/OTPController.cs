using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Autodiscover;
using Microsoft.Exchange.WebServices.Data;
using PORTAL.DAL.EF;
using PORTAL.DAL.EF.Helper;
using PORTAL.DAL.EF.Models;

namespace PORTAL.WEB.Controllers.API
{
    [Produces("application/json")]
    public class OTPController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OTPController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("api/otp/login/{username}")]
        public async Task<IActionResult> PostEmployee(string username)
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
            if(id == string.Empty)
            {
                return new ObjectResult("User not found");
            }
            var otpCodes = await LoginProcess(id);
            var employeeRecord = _context.Employee.Where(a => a.Emp_ID == id).SingleOrDefault();
            SendMail(employeeRecord.E_Mail, otpCodes);
            return new ObjectResult("Success");
        }

        private async Task<string> LoginProcess(string empId)
        {
            string code = GenerateCode();
            var otpEmpId = "";
            var hasOtpCodes = _context.OtpCode.Where(a => a.EmpId == empId).SingleOrDefault();

            if (hasOtpCodes == null)
            {
                OtpCode otpCodemodel = new OtpCode
                {
                    EmpId = empId,
                    Code = code
                };
                var result = _context.OtpCode.Add(otpCodemodel);
                otpEmpId = result.Entity.EmpId;
            }
            else
            {
                hasOtpCodes.Status = Enums.Status.Active;
                otpEmpId = UpdateOtpCode(hasOtpCodes, code);
            }

            await _context.SaveChangesAsync();
            return code;
        }

        private string GenerateCode()
        {
            string pin = string.Empty;
            using (var crypto = new RNGCryptoServiceProvider())
            {
                while (pin == string.Empty)
                {
                    byte[] buffer = new byte[sizeof(UInt64)];
                    crypto.GetBytes(buffer);
                    var num = BitConverter.ToUInt64(buffer, 0);
                    var tempPin = num % 100000;
                    pin = tempPin.ToString("D5");

                }
            }
            return pin;
        }

        [Route("api/otp/code/{code}")]
        public IActionResult validate(string code)
        {
            var otpCode = _context.OtpCode.Where(a => a.Code == code).SingleOrDefault();

            if (otpCode == null)
            {
                return new ObjectResult("Error Code");
            }

            DateTime otpCodeTime = otpCode.ModifiedOn.Value.AddMinutes(10);
            DateTime currentDateTime = DateTime.Now;
            if (otpCodeTime < currentDateTime)
            {
                return new ObjectResult("Expired Codes");
            }
            otpCode.Status = Enums.Status.Inactive;
            UpdateOtpCode(otpCode,"");
            _context.SaveChanges();
            var employee = _context.Employee.Find(otpCode.EmpId);
            return new ObjectResult("Success");
        }

        private string UpdateOtpCode(OtpCode hasOtpCodes, string code)
        {
            hasOtpCodes.Code = code; 
            var result = _context.OtpCode.Update(hasOtpCodes);
            return result.Entity.EmpId;
        }

        public static bool SendMail(string MsgToEMail, string MsgOTPCode)
        {
            try
            {

                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
                // Set user login credentials
                service.Credentials = new WebCredentials("ezportalapp@outlook.com", "ZTs3rv32018");
                try
                {
                    //Set Office 365 Exchange Webserivce Url
                    string serviceUrl = "https://outlook.office365.com/ews/exchange.asmx";
                    service.Url = new Uri(serviceUrl);
                    EmailMessage emailMessage = new EmailMessage(service);
                    emailMessage.Subject = "PIN Code " + MsgOTPCode;

                    var msgBody = "<div style='font-family:Arial; font-size:14px'>" +
                               "Your EZConnect activation code is:</br></br></br>" +
                               "<div style='font-family:Arial; font-size:20px'>" + MsgOTPCode + "</div></br>" +
                               "This code will expire within 10 minutes.<br><br><br>" +
                               "--</br>" +
                               "EZConnect. Connecting EEC Employees made easy.</br></br>" +
                               " </div>";


                    emailMessage.Body = new MessageBody(msgBody);


                    emailMessage.ToRecipients.Add(MsgToEMail);
                    emailMessage.Sensitivity = Sensitivity.Normal;
                    emailMessage.SendAndSaveCopy();
                }
                catch (AutodiscoverRemoteException exception)
                {
                    // handle exception
                    throw exception;
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}