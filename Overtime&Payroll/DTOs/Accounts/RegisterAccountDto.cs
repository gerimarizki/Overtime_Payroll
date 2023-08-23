using server.Utilities.Enums;

namespace server.DTOs.Accounts
{
    public class RegisterAccountDto
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderLevel Gender { get; set; }
        public DateTime HiringDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ManagerGuid { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Salary { get; set; }
    }
}
