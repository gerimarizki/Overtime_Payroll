using server.Models;
using server.Utilities.Handlers;

namespace server.DTOs.Accounts
{
    public class CreateAccountDto
    {
        public Guid Guid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int? Otp { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? ExpiredTime { get; set; }


        public static implicit operator Account(CreateAccountDto newAccountDto)
        {
            return new()
            {
                Guid = newAccountDto.Guid,
                Email = newAccountDto.Email,
                Password = HandlerForHashing.Hash(newAccountDto.Password),
                IsActive = newAccountDto.IsActive,
                OTP = newAccountDto.Otp,
                IsUsed = newAccountDto.IsUsed,
                ExpiredTime = newAccountDto.ExpiredTime,
                CreatedDate = DateTime.UtcNow
            };
        }

        public static explicit operator CreateAccountDto(Account account)
        {
            return new()
            {
                Guid = account.Guid,
                Email = account.Email,
                Password = account.Password,
                IsActive = account.IsActive,
                Otp = account.OTP,
                IsUsed = account.IsUsed,
                ExpiredTime = account.ExpiredTime
            };
        }
    }
}
