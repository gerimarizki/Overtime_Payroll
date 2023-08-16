using Microsoft.AspNetCore.SignalR;
using Overtime_Payroll.Utilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime_Payroll.Models
{
    [Table("tb_m_employees")]
    public class Employee : BaseEntity
    {
       
        [Column("nik", TypeName = "nchar(6)")]
        public string NIK { get; set; }
        [Column("first_name", TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }
        [Column("last_name", TypeName = "nvarchar(100)")]
        public string? LastName { get; set; }
        [Column("birth_date")]
        public DateTime BirthDate { get; set; }
        [Column("gender", TypeName = "char(1)")]
        public string Gender { get; set; }
        [Column("hiring_date")]
        public DateTime HiringDate { get; set; }
        [Column("email", TypeName = "nvarchar(100)")]
        public string Email { get; set; }
        [Column("phone_number", TypeName = "nvarchar(20)")]
        public string PhoneNumber { get; set; }
        [Column("report_to")]
        public Guid? ReportTo { get; set; }
        [Column("employee_level_id")]
        public Guid EmployeeLevelId { get; set; }
        [Column("overtime_id")]
        public Guid OvertimeId { get; set; }
        [Column("department_id")]
        public Guid DepartmentId { get; set; }

        //Cardinality
        public Account? Account { get; set; }
        public EmployeeLevel? EmployeeLevel { get; set; }
        public Department? Department { get; set; }
        public ICollection<Overtime>? Overtimes { get; set; }
        public ICollection<Payroll>? Payrolls { get; set; }
    }
}
