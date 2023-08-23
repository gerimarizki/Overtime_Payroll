using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.AccountRoles;
using server.DTOs.Employees;
using server.DTOs.HistoriesOvertime;
using server.Models;
using server.Services;
using server.Utilities.Handlers;
using System.Diagnostics;
using System.Net;

namespace Client.Controllers
{
    public class HistoryOvertimeController : Controller
    {
        private readonly IHistoryOvertimeRepository repository;

        public HistoryOvertimeController(IHistoryOvertimeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await repository.Get();
            var ListHistoryOvertime = new List<HistoryOvertime>();

            if (result.Data != null)
            {
                ListHistoryOvertime = result.Data.ToList();
            }
            return View(ListHistoryOvertime);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHistoryOvertimeDto newHistoryOvertime)
        {

            var result = await repository.Post(newHistoryOvertime);
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
            var ListHistoryOvertime = new UpdateHistoryOvertimeDto();

            if (result.Data != null)
            {
                ListHistoryOvertime = (UpdateHistoryOvertimeDto)result.Data;
            }
            return View(ListHistoryOvertime);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HistoryOvertime historyOvertime) //(model, parameter)
        {
            var result = await repository.Put(historyOvertime.Guid, historyOvertime);

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
    }
}