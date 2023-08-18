using Overtime_Payroll.Models;

namespace Overtime_Payroll.DTOs.Payrolls
{
    public class UpdatePayrollDto
    {
        public Guid Guid { get; set; }
        public double Salary { get; set; }
        public Guid EmployeeGuid { get; set; }


        public static implicit operator Payroll(UpdatePayrollDto payrollDto)
        {
            return new()
            {
                Guid = payrollDto.Guid,
                Salary = payrollDto.Salary,
                Allowance = payrollDto.Salary * 3 / 100,
                EmployeeGuid = payrollDto.EmployeeGuid,
            };
        }

        public static explicit operator UpdatePayrollDto(Payroll payroll)
        {
            return new()
            {
                Guid = payroll.Guid,
                Salary = payroll.Salary,
                EmployeeGuid = payroll.EmployeeGuid,
            };
        }
    }
}
