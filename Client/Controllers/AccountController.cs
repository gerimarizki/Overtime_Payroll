﻿using server.DTOs.Accounts;
using server.Models;
using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
   
    public class AccountController : Controller
    {

        private readonly IAccountRepository repository;

        public AccountController(IAccountRepository repository)
        {
            this.repository = repository;
        }
        
        public async Task<IActionResult> Index()
        {
            var result = await repository.Get();
            var ListAccount = new List<Account>();

            if (result.Data != null)
            {
                ListAccount = result.Data.ToList();
            }
            return View(ListAccount);
        }
        [HttpGet]
        //[AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            HttpContext.Session.Remove("JWToken");
            return View();
        }

        [HttpPost]
        //[AllowAnonymous]
        public async Task<IActionResult> Login(LoginAccountDto login)
        {
            var result = await repository.Login(login);
            if (result is null)
            {
                TempData["Error"] = $"Failed to Login! - {result.Message}!";
                return RedirectToAction("Login", "Account");
            }
            else if (result.Code == 409)
            {
                TempData["Error"] = $"Failed to Login! - {result.Message}!";
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            else if (result.Code == 200)
            {
                TempData["Success"] = $"Successfully Login! - {result.Data}!";
                HttpContext.Session.SetString("JWToken", result.Data);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountDto newaccount)
        {

            var result = await repository.Post(newaccount);
            if (result.Status == "200")
            {
                TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
                return RedirectToAction(nameof(Index));
            }
            else if (result.Status == "409")
            {
                TempData["Error"] = $"Data failed Registered! - {result.Message}!";
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await repository.Get(id);
            var ListAccount = new UpdateAccountDto();

            if (result.Data != null)
            {
                ListAccount = (UpdateAccountDto)result.Data;
            }
            return View((UpdateAccountDto)ListAccount);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Account account)
        {
            var result = await repository.Put(account.Guid, account);

            if (result.Code == 200)
            {
                TempData["Success"] = $"Data has been Successfully Edited! - {result.Message}!";
                return RedirectToAction("Index", "Account");
            }
            return RedirectToAction(nameof(Edit));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var result = await repository.Delete(guid);
            if (result.Code == 200)
            {
                TempData["Success"] = $"Data has been Successfully Deleted! - {result.Message}!";
            }
            else
            {
                TempData["Error"] = $"Data failed Deleted! - {result.Message}!";
            }
            return RedirectToAction(nameof(Index));
        }


    }
}