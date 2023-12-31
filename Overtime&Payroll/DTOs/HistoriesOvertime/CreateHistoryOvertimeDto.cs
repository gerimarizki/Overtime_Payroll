﻿using server.Models;

namespace server.DTOs.HistoriesOvertime

{
    public class CreateHistoryOvertimeDto
    {
        public Guid OvertimeGuid { get; set; }


        public static implicit operator HistoryOvertime(CreateHistoryOvertimeDto newHistoryOvertimeDto)
        {
            return new()
            {
                OvertimeGuid = newHistoryOvertimeDto.OvertimeGuid,
            };
        }

        public static explicit operator CreateHistoryOvertimeDto(HistoryOvertime historyOvertime)
        {
            return new()
            {
                OvertimeGuid = historyOvertime.OvertimeGuid
            };
        }
    }
}
