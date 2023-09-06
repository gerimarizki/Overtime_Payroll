using Client.Contracts;
using Client.Models;
using Client.ViewModels.Payroll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.AccountRoles;
using server.DTOs.Employees;
using server.DTOs.Overtimes;
using server.DTOs.Payrolls;
using server.Models;
using server.Services;
using server.Utilities.Handlers;
using System.Data;
using System.Diagnostics;
using System.Net;

namespace Client.Controllers
{
    [Authorize(Roles = "Finance, Employee")]
    public class PayrollController : Controller
    {
        private readonly IPayrollRepository repository;

        public PayrollController(IPayrollRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await repository.Get();
            var ListPayroll = new List<Payroll>();

            if (result.Data != null)
            {
                ListPayroll = result.Data.ToList();
            }
            return View(ListPayroll);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePayrollDto createPayrollDto)
        {

            var result = await repository.Post(createPayrollDto);
            if (result.Status == "200")
            {
                TempData["Success"] = "Data berhasil masuk";
                return RedirectToAction(nameof(Index));
            }
            else if (result.Status == "409")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await repository.Get(id);
            var ListPayroll = new UpdatePayrollDto();

            if (result.Data != null)
            {
                ListPayroll = (UpdatePayrollDto)result.Data;
            }
            return View(ListPayroll);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Payroll payroll) //(model, parameter)
        {
            var result = await repository.Put(payroll.Guid, payroll);

            if (result.Code == 200)
            {
                TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
                return RedirectToAction("Index", "Employee");
            }
            return RedirectToAction(nameof(Edit));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var result = await repository.Delete(guid);
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

        public IActionResult PayslipDetail() { return View(); }
    }
}