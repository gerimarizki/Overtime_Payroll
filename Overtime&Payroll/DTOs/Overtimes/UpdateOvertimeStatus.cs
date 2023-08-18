using Overtime_Payroll.Utilities.Enums;

namespace Overtime_Payroll.DTOs.Overtimes
{
    public class UpdateOvertimeStatus
    {
        public Guid Guid { get; set; }
        public StatusLevel Status { get; set; }
    }
}

