using PORTAL.DAL.EF.Helper;
using PORTAL.DAL.EF.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PORTAL.DAL.EF.Models
{
    public class OtpCode : IAuditable
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Enums.Status Status { get; set; }
        public string EmpId { get; set; }
        [ForeignKey("EmpId")]
        public Employee Employee { get; set; }
    }
}
