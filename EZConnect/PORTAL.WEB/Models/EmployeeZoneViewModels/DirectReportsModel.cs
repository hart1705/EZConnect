using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PORTAL.WEB.Models.EmployeeZoneViewModels
{
    public class DirectReportsModel
    {
        public string Emp_ID { get; set; }
        public string Employee_Name_English { get; set; }
        public string Employee_Name_Arabic { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string E_Mail { get; set; }
        public string Hiring_Date { get; set; }
        public string Grade { get; set; }

    }
}
