using Overtime_Payroll.Models;

namespace Overtime_Payroll.DTOs.HistoriesOvertime
{
    public class UpdateHistoryOvertimeDto
    {
        public Guid Guid { get; set; }

        public Guid OvertimeGuid { get; set; }

        public static implicit operator HistoryOvertime(UpdateHistoryOvertimeDto updateHistoryOvertimeDto)
        {
            return new()
            {
                Guid = updateHistoryOvertimeDto.Guid,
                OvertimeGuid = updateHistoryOvertimeDto.OvertimeGuid
            };
        }

        public static explicit operator UpdateHistoryOvertimeDto(HistoryOvertime history)
        {
            return new()
            {
                Guid = history.Guid,
                OvertimeGuid = history.OvertimeGuid
            };
        }
    }
}
