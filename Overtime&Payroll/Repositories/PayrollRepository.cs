﻿using Overtime_Payroll.Contracts;
using Overtime_Payroll.Data;
using Overtime_Payroll.Models;

namespace Overtime_Payroll.Repositories
{
    public class PayrollRepository : GeneralRepository<Payroll>, IPayrollRepository
    {
        public PayrollRepository(OvertimeDbContext context) : base(context) { }
    }
}