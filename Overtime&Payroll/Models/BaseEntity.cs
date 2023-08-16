using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Overtime_Payroll.Models
{
    public abstract class BaseEntity
    {
        [Key]
        [Column("guid")]
        public Guid Guid { get; set; }
    }
}
