using server.Utilities.Enums;

namespace server.DTOs.Overtimes
{
    public class UpdateOvertimeStatus
    {
        public Guid Guid { get; set; }
        public StatusLevel Status { get; set; }
    }
}

