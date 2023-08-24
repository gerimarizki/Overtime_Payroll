using server.Models;

namespace server.DTOs.Payrolls
{
    public class CreatePayrollDto
    {
        public double Salary { get; set; }
        public double Allowance { get; set; }
        public Guid EmployeeGuid { get; set; }

        public static implicit operator Payroll(CreatePayrollDto newPayrolls)
        {
            return new()
            {
                Guid = new Guid(),
                Salary = newPayrolls.Salary,
                Allowance = newPayrolls.Allowance,
                EmployeeGuid = newPayrolls.EmployeeGuid,
            };
        }

        public static explicit operator CreatePayrollDto(Payroll payroll)
        {
            return new()
            {
                Allowance = payroll.Allowance,
                Salary = payroll.Salary,
                EmployeeGuid = payroll.EmployeeGuid
            };
        }

    }
}
