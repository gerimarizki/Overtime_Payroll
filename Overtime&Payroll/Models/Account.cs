using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime_Payroll.Models
{
    [Table("tb_m_accounts")]
    public class Account : BaseEntity
    {
        [Column("email", TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Column("password", TypeName = "nvarchar(30)")]
        public string Password { get; set; }

        [Column("otp")]
        public int? OTP { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("expired_time")]
        public DateTime? ExpiredTime { get; set; }


        // Kardinalitas Account
        public ICollection<AccountRole>? AccountRoles { get; set; }
        public Employee? Employee { get; set; }



    }
}
