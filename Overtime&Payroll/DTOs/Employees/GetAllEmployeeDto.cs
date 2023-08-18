using Overtime_Payroll.Utilities.Enums;

namespace Overtime_Payroll.DTOs.Employees
{
    public class GetAllEmoployeeDto
    {
        public Guid Guid { get; set; }
        public string NIK { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public GenderLevel Gender { get; set; }
        public DateTime HiringDate { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ManagerGuid { get; set; }
        public string? Manager { get; set; }
        public string RoleName { get; set; }
        public double Salary { get; set; }
    }
}