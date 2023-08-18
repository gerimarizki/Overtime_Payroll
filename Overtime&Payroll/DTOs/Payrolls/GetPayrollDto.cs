using Overtime_Payroll.Models;

namespace Overtime_Payroll.DTOs.Payrolls
{
    public class GetPayrollDto
    {
        public Guid Guid { get; set; }
        public DateTime PayDate { get; set; }
        public double Salary { get; set; }
        public double Allowace { get; set; }
        public Guid EmployeeGuid { get; set; }
        public string EmployeeName { get; set; }


        public static implicit operator Payroll(GetPayrollDto payslipDto)
        {
            return new()
            {
                Guid = payslipDto.Guid,
                PayDate = DateTime.Now,
                Salary = payslipDto.Salary,
                Allowance = payslipDto.Salary * 3 / 100,
                EmployeeGuid = payslipDto.EmployeeGuid,
            };
        }

        public static explicit operator GetPayrollDto(Payroll payslip)
        {
            return new()
            {
                Guid = payslip.Guid,
                PayDate = DateTime.Now,
                Salary = payslip.Salary,
                Allowace = payslip.Salary * 3 / 100,
                EmployeeGuid = payslip.EmployeeGuid,
            };
        }
    }
}