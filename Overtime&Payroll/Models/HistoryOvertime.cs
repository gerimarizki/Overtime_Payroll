using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime_Payroll.Models
{
    [Table("tb_tr_histories_overtimes")]
    public class HistoryOvertime : BaseEntity
    {
        [Column("overtime_guid")]
        public Guid OvertimeGuid { get; set; }

        // Cardinality
        public Overtime? Overtime { get; set; }

    }
}
