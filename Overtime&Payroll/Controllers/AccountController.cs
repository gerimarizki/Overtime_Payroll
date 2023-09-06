using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.Accounts;
using server.DTOs.Payrolls;
using server.Services;
using server.Utilities.Handlers;
using System.Net;

namespace server.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    [Authorize]
    [EnableCors]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("Profilall/{guid}")]
        public IActionResult GetAllDetailProfil(Guid guid)
        {
            var profil = _service.GetAllDetailProfil(guid);
            if (profil == null)
                return NotFound(new HandlerForResponse<string>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Profil not found"
                });
            return Ok(new HandlerForResponse<IEnumerable<GetDetailProfilDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Profil Retrieved",
                Data = profil

            });
        }

        [AllowAnonymous]
        [HttpGet("Profil/{guid}")]
        public IActionResult GetDetailProfil(Guid guid)
        {
            var profil = _service.GetDetailProfil(guid);
            if (profil == null)
                return NotFound(new HandlerForResponse<string>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Profil not found"
                });
            return Ok(new HandlerForResponse<GetDetailProfilDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Profil Retrieved",
                Data = profil

            }) ;
        }

        // Register
        [HttpPost("Register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterAccountDto register)
        {
            var isCreated = _service.RegistrationAccount(register);
            if (!isCreated)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<CreateAccountDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<CreateAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Register Success"
            });
        }

        // Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login(LoginAccountDto login)
        {
            var loginResult = _service.LoginAccount(login);
            if (loginResult == "0")
                return NotFound(new HandlerForResponse<string>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Account not found"
                });

            if (loginResult == "-1")
                return BadRequest(new HandlerForResponse<string>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Password is incorrect"
                });

            if (loginResult == "-2")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<string>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving when creating token"
                });
            }

            return Ok(new HandlerForResponse<string>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Login Success",
                Data = loginResult
            });
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public IActionResult ForgetPassword(ForgotPasswordAccountDto forgotPasswordDto)
        {
            var isUpdated = _service.ForgotPasswordAccountDto(forgotPasswordDto);
            if (isUpdated is 0)
                return NotFound(new HandlerForResponse<ForgotPasswordAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Email not found"
                });

            if (isUpdated is -1)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<ForgotPasswordAccountDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<ForgotPasswordAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Otp has been sent to your email"
            });
        }


        [HttpPost("change-password")]
        [AllowAnonymous]
        public IActionResult ChangePassword(ChangePasswordAccountDto changePasswordDto)
        {
            var isUpdated = _service.ChangePassword(changePasswordDto);
            if (isUpdated == 0)
                return NotFound(new HandlerForResponse<UpdateAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Email not found"
                });

            if (isUpdated == -1)
            {
                return BadRequest(new HandlerForResponse<UpdateAccountDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Otp is already used"
                });
            }

            if (isUpdated == -2)
            {
                return BadRequest(new HandlerForResponse<UpdateAccountDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Otp is incorrect"
                });
            }

            if (isUpdated == -3)
            {
                return BadRequest(new HandlerForResponse<UpdateAccountDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Otp is expired"
                });
            }

            if (isUpdated is -4)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<UpdateAccountDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<UpdateAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Password has been changed successfully"
            });
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetAccount();

            if (!entities.Any())
            {
                return NotFound(new HandlerForResponse<GetAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<GetAccountDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var account = _service.GetAccountByGuid(guid);
            if (account is null)
                return NotFound(new HandlerForResponse<GetAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });

            return Ok(new HandlerForResponse<GetAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = account
            });
        }

        [HttpPost]
        public IActionResult Create(CreateAccountDto newAccountDto)
        {
            var createdAccount = _service.CreateAccount(newAccountDto);
            if (createdAccount is null)
                return BadRequest(new HandlerForResponse<GetAccountDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });

            return Ok(new HandlerForResponse<GetAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createdAccount
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateAccountDto updateAccountDto)
        {
            var update = _service.UpdateAccount(updateAccountDto);
            if (update is -1)
                return NotFound(new HandlerForResponse<UpdateAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "ID not found"
                });

            if (update is 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<UpdateAccountDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<UpdateAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteAccount(guid);

            if (delete == -1)
                return NotFound(new HandlerForResponse<GetAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });

            if (delete == 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<GetAccountDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<GetAccountDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }

    }
}