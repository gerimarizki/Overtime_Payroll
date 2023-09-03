using server.Models;
using server.Utilities.Enums;

public class TestOvertimeDto
{
    public DateTime StartOvertimeDate { get; set; }
    public DateTime EndOvertimeDate { get; set; }
    public string? Remarks { get; set; }
    //public StatusLevel Status { get; set; }
    public Guid EmployeeGuid { get; set; }

    public static implicit operator Overtime(TestOvertimeDto newOver)
    {
        return new()
        {
            Guid = new Guid(),
            StartOvertimeDate = newOver.StartOvertimeDate,
            EndOvertimeDate = newOver.EndOvertimeDate,
            Remarks = newOver.Remarks,
            //Status = StatusLevel.Waiting,
            EmployeeGuid = newOver.EmployeeGuid
        };
    }

    public static explicit operator TestOvertimeDto(Overtime overtime)
    {
        return new()
        {
            StartOvertimeDate = overtime.StartOvertimeDate,
            EndOvertimeDate = overtime.EndOvertimeDate,
            Remarks = overtime.Remarks,
            EmployeeGuid = overtime.EmployeeGuid
        };

    }
}