using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime_Payroll.Models
{
    [Table("tb_m_accounts")]
    public class Account : BaseEntity
    {
        [Column("password", TypeName = "nvarchar(30)")]
        public string Password { get; set; }
        [Column("is_deleted")]
        public bool Isdeleted { get; set; }
        [Column("otp")]
        public int OTP { get; set; }
        [Column("is_used")]
        public bool? IsUsed { get; set; }
        [Column("expired_time")]
        public DateTime? ExpiredTime { get; set; }
        [Column("employee_id")]
        public Guid EmployeeId { get; set; }

        // kardinalitas account
        public ICollection<AccountRole>? AccountRoles { get; set; }
        public Employee? Employee { get; set; }

    }
}
