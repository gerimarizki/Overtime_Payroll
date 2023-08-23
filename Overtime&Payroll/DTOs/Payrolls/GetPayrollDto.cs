using server.Models;

namespace server.DTOs.Payrolls
{
    public class GetPayrollDto
    {
        public Guid Guid { get; set; }
        public DateTime PayDate { get; set; }
        public double Salary { get; set; }
        public double Allowance { get; set; }
        public Guid EmployeeGuid { get; set; }
        public string EmployeeName { get; set; }


        public static implicit operator Payroll(GetPayrollDto payrollDto)
        {
            return new()
            {
                Guid = payrollDto.Guid,
                PayDate = DateTime.Now,
                Salary = payrollDto.Salary,
                Allowance = payrollDto.Salary * 3 / 100,
                EmployeeGuid = payrollDto.EmployeeGuid,
            };
        }

        public static explicit operator GetPayrollDto(Payroll payroll)
        {
            return new()
            {
                Guid = payroll.Guid,
                PayDate = DateTime.Now,
                Salary = payroll.Salary,
                Allowance = payroll.Salary * 3 / 100,
                EmployeeGuid = payroll.EmployeeGuid,
            };
        }
    }
}