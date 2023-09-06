using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.AccountRoles;
using server.DTOs.Overtimes;
using server.Services;
using server.Utilities.Handlers;
using System.Net;

namespace server.Controllers
{
    [ApiController]
    [Route("api/overtimes")]
    [Authorize]
    [EnableCors]
    public class OvertimeController : ControllerBase
    {
        private readonly OvertimeService _service;

        public OvertimeController(OvertimeService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("CountStatus")]
        public IActionResult GetAllStatus()
        {
            var status = _service.CountStatus(); // ini dirubah dari GetOvertime

            if (status == null)
            {
                return NotFound(new HandlerForResponse<GetCountedStatusDto> //dari GetOvertimeDto
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<GetCountedStatusDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = status
            });
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var over = _service.GetAllTestOvertimeToEmployee(); // ini dirubah dari GetOvertime

            if (!over.Any())
            {
                return NotFound(new HandlerForResponse<AllRemainingOvertimeDto> //dari GetOvertimeDto
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<AllRemainingOvertimeDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = over
            });
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create(CreateOvertimeDto newOvertime)
        {
            var over = _service.CreateOvertime(newOvertime);
            if (over is null)
            {
                return BadRequest(new HandlerForResponse<GetOvertimeDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Unsuccesful to Create"
                });
            }

            return Ok(new HandlerForResponse<GetOvertimeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfull to Create",
                Data = over
            });
        }
        [AllowAnonymous]
        [HttpPut]
        public IActionResult Update(UpdateOvertimeDto updateOvertime)
        {
            var over = _service.UpdateOvertime(updateOvertime);
            if (over == -1)
            {
                return NotFound(new HandlerForResponse<UpdateOvertimeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "ID Not Found!!"
                });
            }
            else if (over == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<UpdateOvertimeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error to retrieving data from database"
                });
            }
            else
            {
                return Ok(new HandlerForResponse<UpdateOvertimeDto>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Successfull to update",
                });
            }

        }
        [AllowAnonymous]
        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var overDelete = _service.DeleteOvertime(guid);
            if (overDelete == -1)
            {
                return NotFound(new HandlerForResponse<GetOvertimeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            else if (overDelete == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<GetOvertimeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error to delete data from the database"
                });
            }
            else return Ok(new HandlerForResponse<GetOvertimeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfull to delete"
            });

        }
        [AllowAnonymous]
        [HttpGet("{guid}")]
        public IActionResult GetOvertimeByGuid(Guid guid)
        {
            var getOvertimeByGuid = _service.GetOvertimeByGuid(guid);
            if (getOvertimeByGuid is null)
                return NotFound(new HandlerForResponse<GetOvertimeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });

            return Ok(new HandlerForResponse<GetOvertimeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = getOvertimeByGuid
            });
        }
        [AllowAnonymous]
        [HttpGet("employee-overtime{guid}")]
        public IActionResult GetAllByGuidEmp(Guid guid)
        {
            var over = _service.GetAllByGuidEmp(guid);
            if (over is null)
            {
                return NotFound(new HandlerForResponse<AllRemainingOvertimeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            return Ok(new HandlerForResponse<IEnumerable<AllRemainingOvertimeDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = over
            });
        }
        [AllowAnonymous]
        [HttpGet("manager/{guid}")]
        public IActionResult GetAllByGuidManager(Guid guid)
        {
            var over = _service.GetAllByGuidManager(guid);
            if (over is null)
            {
                return NotFound(new HandlerForResponse<AllRemainingOvertimeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            return Ok(new HandlerForResponse<IEnumerable<AllRemainingOvertimeDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = over
            });
        }
        [AllowAnonymous]
        [HttpPut("update-employee-status")]
        public IActionResult UpdateOvertime(UpdateOvertimeStatus updateOvertime)
        {
            var over = _service.UpdateStatusRemaining(updateOvertime);
            if (over == -1)
            {
                return NotFound(new HandlerForResponse<UpdateOvertimeStatus>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "ID Not Found!!"
                });
            }
            else if (over == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<UpdateOvertimeStatus>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error to retrieving data from database"
                });
            }
            else
            {
                return Ok(new HandlerForResponse<UpdateOvertimeStatus>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Successfull to update",
                });
            }

        }

        //Tambahan baru 24/08/2023
        [AllowAnonymous]
        [HttpGet("get-all-overtime-employee")]
        public IActionResult GetAllOvertimeEmployee()
        {
            var over = _service.GetAllTestOvertimeToEmployee();

            if (!over.Any())
            {
                return NotFound(new HandlerForResponse<AllRemainingOvertimeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<AllRemainingOvertimeDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = over
            });
        }

        [AllowAnonymous]
        [HttpPost("create-overtime-to-employee-testing")]
        public IActionResult CreateOvertimeEmployeeTesting(TestOvertimeDto newOvertime)
        {

            var over = _service.CreateOvertimeToEmployee(newOvertime);
            if (over is null)
            {
                return BadRequest(new HandlerForResponse<AllRemainingOvertimeDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Unsuccesful to Create"
                });
            }

            return Ok(new HandlerForResponse<AllRemainingOvertimeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfull to Create",
                Data = over
            });
        }
        //tutup
    }
}