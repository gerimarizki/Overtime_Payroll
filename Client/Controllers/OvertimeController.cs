using Client.Contracts;
using Client.Models;
using Client.ViewModels.Overtimes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.AccountRoles;
using server.DTOs.Employees;
using server.DTOs.Overtimes;
using server.Models;
using server.Services;
using server.Utilities.Enums;
using server.Utilities.Handlers;
using System.Data;
using System.Diagnostics;
using System.Net;

namespace Client.Controllers
{
    public class OvertimeController : Controller
    {
        public OvertimeController(IOvertimeRepository repository, IEmployeeRepository employeeRepository)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
        }


        private readonly IOvertimeRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;

        //public OvertimeController(IOvertimeRepository repository)
        //{
        //    this.repository = repository;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    var result = await repository.Get();
        //    var ListOvertime = new List<Overtime>();

        //    if (result.Data != null)
        //    {
        //        ListOvertime = result.Data.ToList();
        //    }
        //    return View(ListOvertime);
        //}

        //[HttpGet]
        //public IActionResult Create() 
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateOvertimeDto newOvertimeDto)
        //{

        //    var result = await repository.Post(newOvertimeDto);
        //    if (result.Status == "200")
        //    {
        //        TempData["Success"] = "Data berhasil masuk";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else if (result.Status == "409")
        //    {
        //        ModelState.AddModelError(string.Empty, result.Message);
        //        return View();
        //    }
        //    return RedirectToAction(nameof(Index));

        //}

        //[HttpGet]
        //public async Task<IActionResult> Edit(Guid id)
        //{
        //    var result = await _repository.Get(id);
        //    var ListOvertime = new UpdateOvertimeDto();

        //    if (result.Data != null)
        //    {
        //        ListOvertime = (UpdateOvertimeDto)result.Data;
        //    }
        //    return View(ListOvertime);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(Overtime overtime) //(model, parameter)
        //{
        //    var result = await _repository.Put(overtime.Guid, overtime);

        //    if (result.Code == 200)
        //    {
        //        TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
        //        return RedirectToAction("Index", "Employee");
        //    }
        //    return RedirectToAction(nameof(Edit));
        //}

        [HttpPost]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var result = await _repository.Delete(guid);
            if (result.Status == "200")
            {
                TempData["Success"] = "Data Berhasil Dihapus";
            }
            else
            {
                TempData["Error"] = "Gagal Menghapus Data";
            }
            return RedirectToAction(nameof(Index));
        }


 
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var EmployeeGuid = User.Claims.FirstOrDefault(x => x.Type == "Guid")?.Value;
            var guid = Guid.Parse(EmployeeGuid);
            var employee = await _employeeRepository.Get(guid);
            var result = await _repository.Get();
            var listHistory = new List<RequestOvertimeViewModel>();
            if (result.Data != null)
            {
                listHistory = result.Data.ToList();
            }

            if (User.IsInRole("Finance") || User.IsInRole("Manager"))
            {
                return View(listHistory);
            }

            var newListHistory = new List<RequestOvertimeViewModel>();
            foreach (var history in listHistory)
            {
                if (history.EmployeeGuid == employee.Data.Guid)
                {
                    newListHistory.Add(history);
                }
            }


            return View(newListHistory);
        }


        [HttpGet]
        //[Authorize(Roles = "")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = "")]
        public async Task<IActionResult> Create(RequestOvertimeViewModel requestOvertimeViewModel)
        {
            var result = await _repository.Post(requestOvertimeViewModel);
            if (result.Code == 200)
            {
                TempData["Success"] = "Data berhasil masuk";
                return RedirectToAction("Index", "Overtime");
            }
            else if (result.Status == "400")
            {
                //ModelState.AddModelError(string.Empty, result.Message);
                TempData["Error"] = "Gagal Create";
                return View();
            }
            return RedirectToAction("Create", "Overtime");
        }


        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> Detail(Guid guid)
        {
            var result = await _repository.Get(guid);
            var resultOvertime = new RequestOvertimeViewModel();
            if (result.Data?.Guid is null)
            {
                return View(result);
            }
            else
            {
                resultOvertime.Guid = result.Data.Guid;
                resultOvertime.OvertimeId = result.Data.OvertimeId;
                resultOvertime.FullName = result.Data.FullName;
                resultOvertime.CreatedDate = result.Data.CreatedDate;
                resultOvertime.StartOvertimeDate = result.Data.StartOvertimeDate;
                resultOvertime.EndOvertimeDate = result.Data.EndOvertimeDate;
                resultOvertime.Status = result.Data.Status;
                resultOvertime.PaidOvertime = result.Data.PaidOvertime;
                resultOvertime.Remarks = result.Data.Remarks;
                resultOvertime.OvertimeRemaining = result.Data.OvertimeRemaining;
                resultOvertime.EmployeeGuid = result.Data.EmployeeGuid;
            }

            return View(resultOvertime);
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> ApprovalByManager(RequestOvertimeViewModel requestOvertimeViewModel)
        {
            var approvalManager = new ApprovalByManager();
            switch (requestOvertimeViewModel.Status)
            {
                case StatusLevel.Accepted:
                    approvalManager.Guid = requestOvertimeViewModel.Guid;
                    approvalManager.Status = StatusLevel.Accepted;
                    break;
                case StatusLevel.Rejected:
                    approvalManager.Guid = requestOvertimeViewModel.Guid;
                    approvalManager.Status = StatusLevel.Rejected;
                    break;
                case StatusLevel.Waiting:
                    approvalManager.Guid = requestOvertimeViewModel.Guid;
                    approvalManager.Status = StatusLevel.Waiting;
                    break;
            }

            var repository = await _repository.GetApproval(approvalManager);

            return RedirectToAction("GetManager", "Overtime");
        }
        [HttpGet]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetManager()
        {
            var emp = User.Claims.FirstOrDefault(a => a.Type == "Guid")?.Value;
            var guid = Guid.Parse(emp);
            var result = await _repository.GetOvertimeByManager(guid);
            var employees = new List<RequestOvertimeViewModel>();

            if (result.Data is not null)
            {
                employees = result.Data.ToList();
            }
            return View(employees);
        }
    }


    public class ApprovalByManager
    {
        public Guid Guid { get; set; }
        public StatusLevel Status { get; set; }
    }
}
