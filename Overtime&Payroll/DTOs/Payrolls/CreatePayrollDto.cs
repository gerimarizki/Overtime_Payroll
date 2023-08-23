using server.Models;

namespace server.DTOs.Payrolls
{
    public class CreatePayrollDto
    {
        public double Salary { get; set; }
        public double Allowace { get; set; }
        public Guid EmployeeGuid { get; set; }

        public static implicit operator Payroll(CreatePayrollDto newPayrolls)
        {
            return new()
            {
                Guid = new Guid(),
                Salary = newPayrolls.Salary,
                Allowance = newPayrolls.Allowace,
                EmployeeGuid = newPayrolls.EmployeeGuid,
            };
        }

        public static explicit operator CreatePayrollDto(Payroll payroll)
        {
            return new()
            {
                Allowace = payroll.Allowance,
                Salary = payroll.Salary,
                EmployeeGuid = payroll.EmployeeGuid
            };
        }

    }
}
