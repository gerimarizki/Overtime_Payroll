namespace Client.ViewModels.Payroll
{
    public class GetPayrollViewModel
    {
        public Guid Guid { get; set; }
        public DateTime Date { get; set; }
        public int Salary { get; set; }
        public double Allowance { get; set; }
        public double PaidOvertime { get; set; }
        public double TotalSalary { get; set; }
        public Guid EmployeeGuid { get; set; }
        public string FullName { get; set; }
    }
}
