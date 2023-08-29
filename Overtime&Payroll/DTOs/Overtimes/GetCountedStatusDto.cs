using server.Models;
using server.Utilities.Enums;

namespace server.DTOs.Overtimes
{

    public class GetCountedStatusDto
    {
        
        public int CountAccepted { get; set; }
        public int CountRejected { get; set; }
        public int CountWaiting { get; set; }
      
    }
}
