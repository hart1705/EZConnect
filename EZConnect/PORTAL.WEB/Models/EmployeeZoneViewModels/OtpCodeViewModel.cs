using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PORTAL.WEB.Models.EmployeeZoneViewModels
{
    public class OtpCodeViewModel
    {
        [Required]
        public string Code { get; set; }

    }
}
