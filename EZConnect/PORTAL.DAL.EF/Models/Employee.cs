using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PORTAL.DAL.EF.Models
{
    public class Employee
    {
        [Key]
        public string Emp_ID { get; set; }
        public string Employee_Name_English { get; set; }
        public string Employee_Name_Arabic { get; set; }
        public string Gender { get; set; }
        public string Position { get; set; }
        public string PositionArabic { get; set; }
        public string Grade { get; set; }
        public string Cost_Center { get; set; }
        public string Hiring_Date { get; set; }
        public string Termination_Date { get; set; }
        public string Birth_Date { get; set; }
        public string ID_Iqama { get; set; }
        public string E_Mail { get; set; }
        public string Mobile { get; set; }
        public string Nationality { get; set; }
        public string Sponsor { get; set; }
        public string Line_Manager_No { get; set; }
        public string Line_Manager_Name { get; set; }
        public string Line_Manager_Email { get; set; }
        public string Department { get; set; }
        public string Dept_Head_No { get; set; }
        public string Dept_Head_Name { get; set; }
        public string Dept_Head_Email { get; set; }
        public string Employee_Spouse { get; set; }
        public string Section { get; set; }
        public string Cost_Center_HoD { get; set; }
        public string Function_Head { get; set; }
        public string Division { get; set; }
        public string Line_Manager_Name_AP { get; set; }
        public string Dept_Head_Name_AP { get; set; }
        public string Oracle_Org_Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public OtpCode otpCode { get; set; }
    }
}
