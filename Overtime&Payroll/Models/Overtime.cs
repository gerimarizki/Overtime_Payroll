using Overtime_Payroll.Utilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime_Payroll.Models
{
    [Table("tb_m_overtimes")]
    public class Overtime : BaseEntity
    {
        [Column("startOvertime")]
        public DateTime StartOvertime { get; set; }
        [Column("endOvertime")]
        public DateTime EndOvertime { get; set; }
        [Column("submitDate")]
        public DateTime SubmitDate { get; set; }
        [Column("deskripsi", TypeName = "varchar(100)")]
        public string Deskripsi { get; set; }
        [Column("Paid")]
        public int Paid { get; set; }
        [Column("status")]
        public StatusLevel Status { get; set; }
        [Column("employee_id")]
        public Guid EmployeeId { get; set; }

        //kardinalitas overtime
        public Employee Employee { get; set; }
    }
}
