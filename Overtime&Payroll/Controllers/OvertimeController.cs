using Microsoft.AspNetCore.Mvc;
using Overtime_Payroll.DTOs.Overtimes;
using Overtime_Payroll.Services;
using Overtime_Payroll.Utilities.Handlers;
using System.Net;

namespace Overtime_Payroll.Controllers
{
    [ApiController]
    [Route("api/overtimes")]
    public class OvertimeController : ControllerBase
    {
        private readonly OvertimeService _service;

        public OvertimeController(OvertimeService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var over = _service.GetOvertime();

            if (!over.Any())
            {
                return NotFound(new HandlerForResponse<GetOvertimeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<GetOvertimeDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = over
            });
        }

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

        [HttpGet("employee-overtime/{guid}")]
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
    }
}