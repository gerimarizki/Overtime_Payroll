using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.Roles;
using server.Services;
using server.Utilities.Handlers;
using System.Net;

namespace server.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize]
    [EnableCors]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _service;

        public RoleController(RoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetRole();

            if (!entities.Any())
                return NotFound(new HandlerForResponse<GetRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });

            return Ok(new HandlerForResponse<IEnumerable<GetRoleDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("{guid}")]
        public IActionResult GetRoleByGuid(Guid guid)
        {
            var role = _service.GetRoleByGuid(guid);
            if (role is null)
                return NotFound(new HandlerForResponse<GetRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });

            return Ok(new HandlerForResponse<GetRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = role
            });
        }

        [HttpPost]
        public IActionResult Create(CreateRoleDto newRoleDto)
        {
            var createdRole = _service.CreateRole(newRoleDto);
            if (createdRole is null)
                return BadRequest(new HandlerForResponse<GetRoleDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });

            return Ok(new HandlerForResponse<GetRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createdRole
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateRoleDto updateRoleDto)
        {
            var update = _service.UpdateRole(updateRoleDto);
            if (update is -1)
                return NotFound(new HandlerForResponse<UpdateRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });

            if (update is 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<UpdateRoleDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<UpdateRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteRole(guid);

            if (delete is -1)
                return NotFound(new HandlerForResponse<GetRoleDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });

            if (delete is 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<GetRoleDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<GetRoleDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }
    }
}
