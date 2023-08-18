using Overtime_Payroll.Models;
using Overtime_Payroll.Utilities.Enums;

namespace Overtime_Payroll.DTOs.Overtimes
{

    public class CreateOvertimeDto
    {
        // public string OvertimeNumber { get; set; }
        public DateTime StartOvertimeDate { get; set; }
        public DateTime EndOvertimeDate { get; set; }
        public string Remarks { get; set; }
        public StatusLevel Status { get; set; }
        public Guid EmployeeGuid { get; set; }


        public static implicit operator Overtime(CreateOvertimeDto newOver)
        {
            return new()
            {
                Guid = new Guid(),
                StartOvertimeDate = newOver.StartOvertimeDate,
                EndOvertimeDate = newOver.EndOvertimeDate,
                Remarks = newOver.Remarks,
                Status = newOver.Status,
                EmployeeGuid = newOver.EmployeeGuid
            };
        }

        public static explicit operator CreateOvertimeDto(Overtime overtime)
        {
            return new()
            {
                StartOvertimeDate = overtime.StartOvertimeDate,
                EndOvertimeDate = overtime.EndOvertimeDate,
                Remarks = overtime.Remarks,
                Status = overtime.Status,
                EmployeeGuid = overtime.EmployeeGuid
            };

        }
    }
}
