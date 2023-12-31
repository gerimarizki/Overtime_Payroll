﻿using server.Models;
using server.Utilities.Enums;

namespace server.DTOs.Overtimes
{
    public class AllRemainingOvertimeDto
    {
        public Guid Guid { get; set; }
        public string OvertimeId{ get; set; }
        public DateTime StartOvertimeDate { get; set; }
        public string FullName { get; set; }
        public DateTime EndOvertimeDate { get; set; }
        public double PaidOvertime { get; set; }
        public string? Remarks { get; set; }
        public int OvertimeRemaining { get; set; }
        public StatusLevel Status { get; set; }
        public Guid EmployeeGuid { get; set; }
        public DateTime CreatedDate { get; set; }

        public static implicit operator Overtime(AllRemainingOvertimeDto getOvertime)
        {
            return new()
            {
                Guid = getOvertime.Guid,
                OvertimeId = getOvertime.OvertimeId,
                StartOvertimeDate = DateTime.Today,
                EndOvertimeDate = getOvertime.EndOvertimeDate,
                PaidOvertime = getOvertime.PaidOvertime,
                OvertimeRemaining = getOvertime.OvertimeRemaining,
                Remarks = getOvertime.Remarks,
                Status = getOvertime.Status,
                EmployeeGuid = getOvertime.EmployeeGuid,
            };
        }

        public static explicit operator AllRemainingOvertimeDto(Overtime overtime)
        {
            return new()
            {
                Guid = overtime.Guid,
                OvertimeId = overtime.OvertimeId,
                StartOvertimeDate = overtime.StartOvertimeDate,
                PaidOvertime = overtime.PaidOvertime,
                OvertimeRemaining = overtime.OvertimeRemaining,
                EndOvertimeDate = overtime.EndOvertimeDate,
                Remarks = overtime.Remarks,
                Status = overtime.Status,
                EmployeeGuid = overtime.EmployeeGuid,
            };
        }
    }
}