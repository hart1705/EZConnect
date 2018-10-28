using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PORTAL.WEB.Models.OtpLoginViewModels
{
    public class OtpCodeViewModel
    {
        [Required]
        public string Code { get; set; }

    }
}
