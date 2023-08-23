using Microsoft.AspNetCore.Mvc;
using server.DTOs.AccountRoles;
using server.Services;
using server.Utilities.Handlers;
using System.Net;

namespace server.Controllers
{

    [ApiController]
    [Route("api/account-roles")]
    public class AccountRoleController : ControllerBase
    {
        private readonly AccountRoleService _service;

        public AccountRoleController(AccountRoleService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetAccountRole();

            if (!entities.Any())
            {
                return NotFound(new HandlerForResponse<GetAccountRolesDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<GetAccountRolesDto>>
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
            var accountRole = _service.GetAccountRoleByGuid(guid);
            if (accountRole is null)
                return NotFound(new HandlerForResponse<GetAccountRolesDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });

            return Ok(new HandlerForResponse<GetAccountRolesDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = accountRole
            });
        }

        [HttpPost]
        public IActionResult Create(CreateAccountRoleDto newAccountRoleDto)
        {
            var createdAccountRole = _service.CreateAccountRole(newAccountRoleDto);
            if (createdAccountRole is null)
                return BadRequest(new HandlerForResponse<GetAccountRolesDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });

            return Ok(new HandlerForResponse<GetAccountRolesDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createdAccountRole
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateAccountRoleDto updateAccountRoleDto)
        {
            var update = _service.UpdateAccountRole(updateAccountRoleDto);
            if (update is -1)
                return NotFound(new HandlerForResponse<UpdateAccountRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "ID not found"
                });

            if (update is 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<UpdateAccountRoleDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<UpdateAccountRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteAccountRole(guid);

            if (delete is -1)
                return NotFound(new HandlerForResponse<GetAccountRolesDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });

            if (delete is 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<GetAccountRolesDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<GetAccountRolesDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }
    }
}