using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Overtime_Payroll.Models
{
    public abstract class BaseEntity
    {
        [Key]
        [Column("guid")]
        public Guid Guid { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}
