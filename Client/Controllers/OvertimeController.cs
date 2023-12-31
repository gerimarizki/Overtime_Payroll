﻿using Client.Contracts;
using Client.Models;
using Client.ViewModels.Overtimes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.AccountRoles;
using server.DTOs.Employees;
using server.DTOs.Overtimes;
using server.Models;
using server.Services;
using server.Utilities.Handlers;
using System.Data;
using System.Diagnostics;
using System.Net;

namespace Client.Controllers
{
    [Authorize(Roles = "Finance, Manager, Employee")]
    public class OvertimeController : Controller
    {
        private readonly IOvertimeRepository repository;

        public OvertimeController(IOvertimeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await repository.Get();
            var ListOvertime = new List<Overtime>();

            if (result.Data != null)
            {
                ListOvertime = result.Data.ToList();
            }
            return View(ListOvertime);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestOvertimeDto newOvertimeDto)
        {

            var result = await repository.PostOvertime(newOvertimeDto);
            if (result.Status == "200")
            {
                TempData["Success"] = "Data berhasil masuk";
                return RedirectToAction("Index", "Home");
            }
            else if (result.Status == "409")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await repository.Get(id);
            var ListOvertime = new UpdateOvertimeDto();

            if (result.Data != null)
            {
                ListOvertime = (UpdateOvertimeDto)result.Data;
            }
            return View(ListOvertime);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Overtime overtime) //(model, parameter)
        {
            var result = await repository.Put(overtime.Guid, overtime);

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