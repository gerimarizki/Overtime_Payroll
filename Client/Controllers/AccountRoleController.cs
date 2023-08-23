using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.AccountRoles;
using server.DTOs.Employees;
using server.Models;
using server.Services;
using server.Utilities.Handlers;
using System.Diagnostics;
using System.Net;

namespace Client.Controllers
{
    public class AccountRoleController : Controller
    {
        private readonly IAccountRoleRepository repository;

        public AccountRoleController(IAccountRoleRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await repository.Get();
            var ListAccountRole = new List<AccountRole>();

            if (result.Data != null)
            {
                ListAccountRole = result.Data.ToList();
            }
            return View(ListAccountRole);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountRoleDto newAccountRole)
        {

            var result = await repository.Post(newAccountRole);
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
            var ListAccountRole = new UpdateAccountRoleDto();

            if (result.Data != null)
            {
                ListAccountRole = (UpdateAccountRoleDto)result.Data;
            }
            return View(ListAccountRole);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AccountRole accountRole) //(model, parameter)
        {
            var result = await repository.Put(accountRole.Guid, accountRole);

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