﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PORTAL.WEB.Models.EmployeeZoneViewModels
{
    public class OtpLoginViewModel
    {
        [Required]
        public string UserName { get; set; }

    }
}
