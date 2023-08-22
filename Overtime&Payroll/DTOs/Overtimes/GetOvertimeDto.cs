using Overtime_Payroll.Models;
using Overtime_Payroll.Utilities.Enums;

namespace Overtime_Payroll.DTOs.Overtimes
{
    public class GetOvertimeDto
    {
        public Guid Guid { get; set; }
        public string OvertimeId{ get; set; }
        public DateTime StartOvertimeDate { get; set; }
        public DateTime EndOvertimeDate { get; set; }
        public string? Remarks { get; set; }
        public StatusLevel Status { get; set; }
        public Guid EmployeeGuid { get; set; }
        public string FullName { get; set; }
        public int Remaining { get; set; }
        public double Paid { get; set; }
        public DateTime CreatedDate { get; set; }

        public static implicit operator Overtime(GetOvertimeDto getOvertime)
        {
            return new()
            {
                Guid = getOvertime.Guid,
                OvertimeId = getOvertime.OvertimeId,
                StartOvertimeDate = getOvertime.StartOvertimeDate,
                EndOvertimeDate = getOvertime.EndOvertimeDate,
                Remarks = getOvertime.Remarks,
                Status = getOvertime.Status,
                EmployeeGuid = getOvertime.EmployeeGuid,
                
            };
        }

        public static explicit operator GetOvertimeDto(Overtime overtime)
        {
            return new()
            {
                Guid = overtime.Guid,
                OvertimeId = overtime.OvertimeId,
                StartOvertimeDate = overtime.StartOvertimeDate,
                EndOvertimeDate = overtime.EndOvertimeDate,
                Remarks = overtime.Remarks,
                Status = overtime.Status,
                EmployeeGuid = overtime.EmployeeGuid,
            };
        }

    }
}