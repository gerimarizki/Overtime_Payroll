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

        [Column("first_name", TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("gender")]
        public GenderLevel Gender { get; set; }

        [Column("hiring_date")]
        public DateTime HiringDate { get; set; }

        [Column("phone_number", TypeName = "nvarchar(20)")]
        public string PhoneNumber { get; set; }

        [Column("manager_guid")]
        public Guid? ManagerGuid { get; set; }


        // Kardinalitas Employee
        public Employee? Manager { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public Account? Account { get; set; }

        public Payroll? Payroll { get; set; }

        public ICollection<Overtime>? Overtimes { get; set; }
    }
}
