using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime_Payroll.Models
{
    [Table("tb_tr_payrolls")]
    public class Payroll : BaseEntity
    {
        [Column("pay_date")]
        public DateTime PayDate { get; set; }
        [Column("payroll_cut")]
        public int PayrollCut { get; set; }
        [Column("total_salary")]
        public int TotalSalary { get; set; }
        [Column("employee_id")]
        public Guid EmployeeId { get; set; }

        // kardinalitas payroll
        public Employee? Employee { get; set; }

    }
}
