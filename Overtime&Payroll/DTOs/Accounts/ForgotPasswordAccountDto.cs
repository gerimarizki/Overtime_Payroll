namespace server.DTOs.Accounts
{
    public class ForgotPasswordAccountDto
    {
        public string Email { get; set; }
        public int OTP { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
