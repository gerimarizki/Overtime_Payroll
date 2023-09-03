using Client.ViewModels.Overtimes;
using Client.ViewModels.Payroll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        //private readonly Client.Contracts.IPayrollRepository _payrollRepository;
        //private readonly Client.Contracts.IEmployeeRepository _employeeRepository;
        //private readonly Client.Contracts.IOvertimeRepository _overtimeRepository;

        //public DashboardController(Client.Contracts.IPayrollRepository payrollRepository, Client.Contracts.IEmployeeRepository employeeRepository, Client.Contracts.IOvertimeRepository overtimeRepository)
        //{
        //    _payrollRepository = payrollRepository;
        //    _employeeRepository = employeeRepository;
        //    _overtimeRepository = overtimeRepository;
        //}

        ////[Authorize]
        //public async Task<IActionResult> IndexDashboard()
        //{
        //    var EmployeeGuid = User.Claims.FirstOrDefault(x => x.Type == "Guid")?.Value;
        //    var guid = Guid.Parse(EmployeeGuid);
        //    var salary = await _payrollRepository.Get();
        //    var listSalary = new List<GetPayrollViewModel>();
        //    if (salary.Data is not null)
        //    {
        //        listSalary = salary.Data.ToList();
        //    }

        //    var newListSalary = new GetPayrollViewModel();
        //    foreach (var item in listSalary)
        //    {
        //        if (item.EmployeeGuid == guid)
        //        {
        //            newListSalary.Salary = item.Salary;
        //        }
        //    }
        //    ViewData["salary"] = newListSalary.Salary;
        //    var overtime = await _overtimeRepository.Get();
        //    var listRemaining = new List<RequestOvertimeViewModel>();
        //    if (overtime.Data is not null)
        //    {
        //        listRemaining = overtime.Data.ToList();
        //    }

        //    var newListOvertime = new RequestOvertimeViewModel();
        //    foreach (var item in listRemaining)
        //    {
        //        if (item.EmployeeGuid == guid)
        //        {
        //            newListOvertime.OvertimeRemaining = item.OvertimeRemaining;
        //        }
        //    }
        //    ViewData["remaining"] = newListOvertime.OvertimeRemaining;
        //    var statistic = await _payrollRepository.GetStatistic();
        //    ViewData["statistic"] = statistic.Data;
        //    var countEmployee = await _employeeRepository.GetCount();
        //    ViewData["countEmployee"] = countEmployee.Data;
        //    var totalOvertime = await _payrollRepository.GetStatistic(guid);
        //    ViewData["countOvertime"] = totalOvertime.Data;

        //    var overtimeManager = await _overtimeRepository.Get();
        //    var listOvertimeManager = new List<RequestOvertimeViewModel>();
        //    if (overtimeManager.Data is not null)
        //    {
        //        listOvertimeManager = overtimeManager.Data.ToList();
        //    }

        //    var countOvertimeGuid = listOvertimeManager.Count(item => item.Status == server.Utilities.Enums.StatusLevel.Waiting);
        //    ViewData["countWaiting"] = countOvertimeGuid.ToString();
        //    return View();
        //}
    }
}
