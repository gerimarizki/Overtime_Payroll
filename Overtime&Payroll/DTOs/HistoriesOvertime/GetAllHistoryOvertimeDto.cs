using server.Utilities.Enums;

namespace server.DTOs.HistoriesOvertime
{
    public class GetAllHistoryOvertimeDto
    {
        public Guid Guid { get; set; }
        public string NIK { get; set; }
        public string FullName { get; set; }
        public string OvertimeId { get; set; }
        public DateTime StartOvertimeDate { get; set; }
        public DateTime EndOvertimeDate { get; set; }
        public string? ApproveBy { get; set; }
        public DateTime Submitted { get; set; }
        public Guid? ManagerGuid { get; set; }
        public StatusLevel Status { get; set; }
    }
}
