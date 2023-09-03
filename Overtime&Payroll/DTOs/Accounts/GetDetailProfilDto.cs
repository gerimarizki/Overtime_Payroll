using server.Models;
using server.Utilities.Enums;

namespace server.DTOs.Payrolls
{
    public class GetDetailProfilDto
    {
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string Nik { get; set; }
        public string PhoneNumber { get; set; }
        public double Salary { get; set; }
        public double Allowance { get; set; }
        public double PaidOvertime { get; set; }
        public double TotalSalary { get; set; }
        public int OvertimeRemaining { get; set; }
        public string FullName { get; set; }

        public StatusLevel Status { get; set; }

        public string? OvertimeId { get; set; }

        //public Guid EmployeeGuid { get; set; }
        public int Counter { get; set; }


    }
}
