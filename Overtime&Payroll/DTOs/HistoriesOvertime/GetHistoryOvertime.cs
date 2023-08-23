using server.Models;

namespace server.DTOs.HistoriesOvertime
{
    public class GetHistoryOvertime
    {
        public Guid Guid { get; set; }

        public Guid OvertimeGuid { get; set; }

        public static implicit operator HistoryOvertime(GetHistoryOvertime getHistoryDto)
        {
            return new()
            {
                Guid = getHistoryDto.Guid,
                OvertimeGuid = getHistoryDto.OvertimeGuid
            };
        }

        public static explicit operator GetHistoryOvertime(HistoryOvertime history)
        {
            return new()
            {
                Guid = history.Guid,
                OvertimeGuid = history.OvertimeGuid
            };
        }
    }
}