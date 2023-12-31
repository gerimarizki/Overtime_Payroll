﻿using server.Models;

namespace server.DTOs.Payrolls
{
    public class GetAllPayrollDto
    {
        public Guid Guid { get; set; }
        public DateTime PayDate { get; set; }
        public double Salary { get; set; }
        public double Allowance { get; set; }
        public double PaidOvertime { get; set; }
        public double TotalSalary { get; set; }
        //public double TotalPaidOvertime { get; set; }
        public Guid EmployeeGuid { get; set; }
        public string FullName { get; set; }

        public string OvertimeRemaining { get; set; }

        public static implicit operator Payroll(GetAllPayrollDto payrollDto)
        {
            return new()
            {
                Guid = payrollDto.Guid,
                PayDate = new DateTime(25 / 08 / 2023),
                Salary = payrollDto.Salary,
                Allowance = payrollDto.Salary * 3 / 100,
                EmployeeGuid = payrollDto.EmployeeGuid,   
            };
        }

        public static explicit operator GetAllPayrollDto(Payroll payroll)
        {
            return new()
            {
                Guid = payroll.Guid,               
                PayDate = new DateTime(25/08/2023),
                Salary = payroll.Salary,
                Allowance = payroll.Salary * 3 / 100,
                EmployeeGuid = payroll.EmployeeGuid,

            };
        }




    }
}
