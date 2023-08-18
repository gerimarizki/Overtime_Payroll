namespace Overtime_Payroll.DTOs.Accounts
{
    public class ChangePasswordAccountDto
    {
        public string Email { get; set; }
        public int OTP { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
