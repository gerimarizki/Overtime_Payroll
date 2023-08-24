using server.Utilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    [Table("tb_m_overtimes")]
    public class Overtime : BaseEntity
    {
        [Column("overtime_Id", TypeName = "nchar(8)")]
        public string OvertimeId { get; set; }

        [Column("start_overtime_date")]
        public DateTime StartOvertimeDate { get; set; }

        [Column("end_overtime_date")]
        public DateTime EndOvertimeDate { get; set; }

        [Column("remarks", TypeName = "nvarchar(255)")]
        public string Remarks { get; set; }

        [Column("paid")]
        public double PaidOvertime { get; set; }

        [Column("overtime_remaining")]
        public int OvertimeRemaining { get; set; }

        [Column("status")]
        public StatusLevel Status { get; set; }

        [Column("employee_guid")]
        public Guid EmployeeGuid { get; set; }


        // Kardinalitas Overtime
        public Employee? Employee { get; set; }
        public ICollection<HistoryOvertime>? Histories { get; set; }
    }
}
