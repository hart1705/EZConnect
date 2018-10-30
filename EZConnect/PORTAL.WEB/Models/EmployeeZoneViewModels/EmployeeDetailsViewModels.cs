using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PORTAL.WEB.Models.EmployeeZoneViewModels
{
    public class EmployeeDetailsViewModels
    {
        public string Emp_ID { get; set; }
        public string Employee_Name_English { get; set; }
        public string Employee_Name_Arabic { get; set; }
        public string Gender { get; set; }
        public string Position { get; set; }
        public string PositionArabic { get; set; }
        public string Department { get; set; }
        public string Cost_Center { get; set; }
        public string E_Mail { get; set; }
        public string Mobile { get; set; }
        public IEnumerable<LineManagerModel> LineManager { get; set; }
        public IEnumerable<SharesLineManagerModel> SharesLineManagers { get; set; }
        public IEnumerable<DirectReportsModel> DirectReports { get; set; }




    }
}
