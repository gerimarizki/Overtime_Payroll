using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    [Table("tb_tr_payrolls")]
    public class Payroll : BaseEntity
    {
        [Column("pay_date")]
        public DateTime PayDate { get; set; }

        [Column("allowance")]
        public double Allowance { get; set; }

        [Column("salary")]
        public double Salary { get; set; }

        [Column("employee_guid")]
        public Guid EmployeeGuid { get; set; }

        // Kardinalitas Payroll
        public Employee? Employee { get; set; }

    }
}
