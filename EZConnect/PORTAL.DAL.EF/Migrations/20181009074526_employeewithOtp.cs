using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PORTAL.DAL.EF.Migrations
{
    public partial class employeewithOtp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Emp_ID = table.Column<string>(nullable: false),
                    Birth_Date = table.Column<string>(nullable: true),
                    Cost_Center = table.Column<string>(nullable: true),
                    Cost_Center_HoD = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    Dept_Head_Email = table.Column<string>(nullable: true),
                    Dept_Head_Name = table.Column<string>(nullable: true),
                    Dept_Head_Name_AP = table.Column<string>(nullable: true),
                    Dept_Head_No = table.Column<string>(nullable: true),
                    Division = table.Column<string>(nullable: true),
                    E_Mail = table.Column<string>(nullable: true),
                    Employee_Name_Arabic = table.Column<string>(nullable: true),
                    Employee_Name_English = table.Column<string>(nullable: true),
                    Employee_Spouse = table.Column<string>(nullable: true),
                    Function_Head = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Grade = table.Column<string>(nullable: true),
                    Hiring_Date = table.Column<string>(nullable: true),
                    ID_Iqama = table.Column<string>(nullable: true),
                    Id = table.Column<Guid>(nullable: false),
                    Line_Manager_Email = table.Column<string>(nullable: true),
                    Line_Manager_Name = table.Column<string>(nullable: true),
                    Line_Manager_Name_AP = table.Column<string>(nullable: true),
                    Line_Manager_No = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    Oracle_Org_Name = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    PositionArabic = table.Column<string>(nullable: true),
                    Section = table.Column<string>(nullable: true),
                    Sponsor = table.Column<string>(nullable: true),
                    Termination_Date = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Emp_ID);
                });

            migrationBuilder.CreateTable(
                name: "OtpCode",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    EmpId = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtpCode_Employee_EmpId",
                        column: x => x.EmpId,
                        principalTable: "Employee",
                        principalColumn: "Emp_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtpCode_EmpId",
                table: "OtpCode",
                column: "EmpId",
                unique: true,
                filter: "[EmpId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtpCode");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
