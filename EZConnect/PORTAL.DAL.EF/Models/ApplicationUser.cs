using Microsoft.AspNetCore.Identity;
using PORTAL.DAL.EF.Helper;
using PORTAL.DAL.EF.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PORTAL.DAL.EF.Models
{
    public class ApplicationUser : IdentityUser, IAuditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Enums.Status Status { get; set; }
    }
}
