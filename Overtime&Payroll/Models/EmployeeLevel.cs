using Overtime_Payroll.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime_Payroll.Models
{
    [Table("tb_tr_employee_levels")]
    public class EmployeeLevel : BaseEntity
    {
        [Column("title", TypeName = "nvarchar(50)")]
        public string Title { get; set; }
        [Column("level", TypeName = "nvarchar(50)")]
        public string Level { get; set; }
        [Column("salary")]
        public int Salary { get; set; }
        [Column("allowance")]
        public int Allowance { get; set; }

        //karidnalitas employeelevel
        public ICollection<Employee> Employees { get; set; }
    }
}
