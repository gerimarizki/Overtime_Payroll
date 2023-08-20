using Microsoft.AspNetCore.Mvc;
using Overtime_Payroll.DTOs.HistoriesOvertime;
using Overtime_Payroll.Services;
using Overtime_Payroll.Utilities.Handlers;
using System.Net;

namespace Overtime_Payroll.Controllers
{
    [ApiController]
    [Route("api/historiesOvertime")]
    public class HistoryOvertimeController : ControllerBase
    {
        private readonly HistoryOvertimeService _service;

        public HistoryOvertimeController(HistoryOvertimeService service)
        {
            _service = service;
        }

        [HttpGet("get-all-history-overtime-user")]
        public IActionResult GetAllHistoryOvertime()
        {
            var entities = _service.GetAllHistoriesOvertime();

            if (!entities.Any())
                return NotFound(new HandlerForResponse<GetAllHistoryOvertimeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });

            return Ok(new HandlerForResponse<IEnumerable<GetAllHistoryOvertimeDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var entities = _service.GetHistoryOvertime();

            if (!entities.Any())
                return NotFound(new HandlerForResponse<GetHistoryOvertime>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });

            return Ok(new HandlerForResponse<IEnumerable<GetHistoryOvertime>>
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
            var history = _service.GetHistoryOvertimeByGuid(guid);
            if (history is null)
                return NotFound(new HandlerForResponse<GetHistoryOvertime>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });

            return Ok(new HandlerForResponse<GetHistoryOvertime>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = history
            });
        }

        [HttpPost]
        public IActionResult Create(CreateHistoryOvertimeDto newHistoryOvertimeDto)
        {
            var createdHistory = _service.CreateHistoryOvertime(newHistoryOvertimeDto);
            if (createdHistory is null)
                return BadRequest(new HandlerForResponse<GetHistoryOvertime>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });

            return Ok(new HandlerForResponse<GetHistoryOvertime>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createdHistory
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateHistoryOvertimeDto updateHistoryOvertimeDto)
        {
            var update = _service.UpdateHistoryOvertime(updateHistoryOvertimeDto);
            if (update is -1)
                return NotFound(new HandlerForResponse<UpdateHistoryOvertimeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });

            if (update is 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<UpdateHistoryOvertimeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<UpdateHistoryOvertimeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteHistoryOvertime(guid);

            if (delete is -1)
                return NotFound(new HandlerForResponse<GetHistoryOvertime>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "ID not found"
                });

            if (delete is 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<GetHistoryOvertime>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<GetHistoryOvertime>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }

    }}
