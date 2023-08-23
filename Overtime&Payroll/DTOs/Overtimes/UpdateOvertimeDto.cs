using server.Models;
using server.Utilities.Enums;

namespace server.DTOs.Overtimes
{
    public class UpdateOvertimeDto
    {
        public Guid Guid { get; set; }
        public string OvertimeId { get; set; }
        public DateTime StartOvertimeDate { get; set; }
        public DateTime EndOvertimeDate { get; set; }
        public string? Remarks { get; set; }
        public StatusLevel Status { get; set; }
        public Guid EmployeeGuid { get; set; }

        public static implicit operator Overtime(UpdateOvertimeDto getOvertime)
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

        public static explicit operator UpdateOvertimeDto(Overtime overtime)
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