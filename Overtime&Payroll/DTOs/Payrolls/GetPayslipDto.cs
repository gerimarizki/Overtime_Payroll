using server.Models;
using server.Utilities.Enums;

namespace server.DTOs.Payrolls
{
    public class GetPayslipDto
    {
        public Guid EmployeeGuid { get; set; }
        public string? OvertimeId { get; set; }
        public DateTime PayDate { get; set; }
        public double Salary { get; set; }
        public double Allowance { get; set; }
        public double PaidOvertime { get; set; }
        public double TotalSalary { get; set; }
        public string FullName { get; set; }

        public StatusLevel Status { get; set; }
        public int Counter { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
