using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime_Payroll.Models
{
    [Table("tb_tr_departments")]
    public class Department : BaseEntity
    {
        [Column("name", TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        //kardinalitas department
        public ICollection<Employee> Employee { get; set; }
    }
}
