using server.Models;

namespace server.DTOs.Accounts
{
    public class GetAccountDto
    {
        public Guid Guid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int? OTP { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? ExpiredTime { get; set; }

        public static implicit operator Account(GetAccountDto getAccountDto)
        {
            return new()
            {
                Guid = getAccountDto.Guid,
                Email = getAccountDto.Email,
                Password = getAccountDto.Password,
                IsActive = getAccountDto.IsActive,
                OTP = getAccountDto.OTP,
                IsUsed = getAccountDto.IsUsed,
                ExpiredTime = getAccountDto.ExpiredTime
            };
        }

        public static explicit operator GetAccountDto(Account account)
        {
            return new()
            {
                Guid = account.Guid,
                Email = account.Email,
                Password = account.Password,
                IsActive = account.IsActive,
                OTP = account.OTP,
                IsUsed = account.IsUsed,
                ExpiredTime = account.ExpiredTime
            };
        }
    }
}