using Microsoft.AspNetCore.Mvc;
using server.DTOs.Employees;
using server.Services;
using server.Utilities.Handlers;
using System.Net;

namespace server.Controllers
{

    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;

        public EmployeeController(EmployeeService service)
        {
            _service = service;
        }

        [HttpGet("get-manager-employee")]
        public IActionResult GetManagers()
        {
            var entities = _service.GetManager();

            if (entities == null)
            {
                return NotFound(new HandlerForResponse<GetAllEmoployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<GetAllEmoployeeDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("get-all-employee")]
        public IActionResult GetAllEmployee()
        {
            var entities = _service.GetAllEmployee();

            if (!entities.Any())
            {
                return NotFound(new HandlerForResponse<GetAllEmoployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<GetAllEmoployeeDto>>
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
            var entities = _service.GetEmployee();

            if (!entities.Any())
            {
                return NotFound(new HandlerForResponse<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<GetEmployeeDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = entities
            });
        }

        [HttpGet("get-manager/{guid}")]
        public IActionResult GetMangerByGuid(Guid guid)
        {
            var employee = _service.GetManagerByGuid(guid);
            if (employee is null)
                return NotFound(new HandlerForResponse<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });

            return Ok(new HandlerForResponse<GetEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = employee
            });
        }

        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var employee = _service.GetEmployeeByGuid(guid);
            if (employee is null)
                return NotFound(new HandlerForResponse<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });

            return Ok(new HandlerForResponse<GetEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data found",
                Data = employee
            });
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto newEmployeeDto)
        {
            var createdEmployee = _service.CreateEmployee(newEmployeeDto);
            if (createdEmployee is null)
                return BadRequest(new HandlerForResponse<GetEmployeeDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Data not created"
                });

            return Ok(new HandlerForResponse<GetEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully created",
                Data = createdEmployee
            });
        }

        [HttpPut("update-employee")]
        public IActionResult UpdateEmployee(UpdateAllEmployeeDto updateEmployeeDto)
        {
            var update = _service.UpdateEmployee(updateEmployeeDto);
            if (update is -1)
                return NotFound(new HandlerForResponse<UpdateEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "ID not found"
                });

            if (update is 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<UpdateEmployeeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<UpdateEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpPut]
        public IActionResult Update(UpdateEmployeeDto updateEmployeeDto)
        {
            var update = _service.Update(updateEmployeeDto);
            if (update is -1)
                return NotFound(new HandlerForResponse<UpdateEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "ID not found"
                });

            if (update is 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<UpdateEmployeeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<UpdateEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully updated"
            });
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var delete = _service.DeleteEmployee(guid);

            if (delete is -1)
                return NotFound(new HandlerForResponse<GetEmployeeDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id not found"
                });

            if (delete is 0)
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<GetEmployeeDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error retrieving data from the database"
                });

            return Ok(new HandlerForResponse<GetEmployeeDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfully deleted"
            });
        }


        [HttpGet("total-count")]
        public IActionResult GetTotalEmployeeCount()
        {
            int totalEmployeeCount = _service.GetTotalEmployeeCount();

            return Ok(new HandlerForResponse<int>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Total Employee Count",
                Data = totalEmployeeCount
            });
        }
    }
}
