using Microsoft.AspNetCore.Mvc;
using Overtime_Payroll.DTOs.Payrolls;
using Overtime_Payroll.Services;
using Overtime_Payroll.Utilities.Handlers;
using System.Net;

namespace Overtime_Payroll.Controllers
{
    [ApiController]
    [Route("api/payrolls")]
    public class PayrollController : ControllerBase
    {
        private readonly PayrollService _payrollService;

        public PayrollController(PayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpGet]
        public IActionResult Getpayroll()
        {
            var payroll = _payrollService.GetPayroll();
            if (!payroll.Any())
            {
                return NotFound(new HandlerForResponse<GetPayrollDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<GetPayrollDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = payroll
            });
        }

        [HttpGet("{payrollbyguid}")]
        public IActionResult GetpayrollByGuid(Guid guid)
        {
            var payroll = _payrollService.GetPayrollDtoByGuid(guid);
            if (payroll is null)
            {
                return NotFound(new HandlerForResponse<GetPayrollDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<GetPayrollDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = payroll
            });
        }

        [HttpPost]
        public IActionResult Create(CreatePayrollDto newpayroll)
        {
            var payroll = _payrollService.CreatePayroll(newpayroll);
            if (payroll is null)
            {
                return BadRequest(new HandlerForResponse<GetPayrollDto>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Unsuccessfull to Create"
                });
            }

            return Ok(new HandlerForResponse<GetPayrollDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Successfull to Create"
            });

        }

        [HttpPut]
        public IActionResult Update(UpdatePayrollDto updatepayroll)
        {
            var payroll = _payrollService.UpdatePayrolls(updatepayroll);
            if (payroll == -1)
            {
                return NotFound(new HandlerForResponse<UpdatePayrollDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }
            else if (payroll == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<UpdatePayrollDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error to retrieving data from database"
                });
            }
            else
            {
                return Ok(new HandlerForResponse<UpdatePayrollDto>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Successfull to Update"
                });
            }
        }

        [HttpDelete]
        public IActionResult Delete(Guid guid)
        {
            var payroll = _payrollService.DeletePayroll(guid);
            if (payroll == -1)
            {
                return NotFound(new HandlerForResponse<GetPayrollDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }
            else if (payroll == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new HandlerForResponse<GetPayrollDto>
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error to retrieving data from database"
                });
            }
            else
            {
                return Ok(new HandlerForResponse<GetPayrollDto>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Successfull to Delete"
                });
            }
        }
        [HttpGet("{get-payroll-overtime}")]
        public IActionResult GetpayrollOver()
        {
            var payroll = _payrollService.GetAllMasterOver();
            if (!payroll.Any())
            {
                return NotFound(new HandlerForResponse<GetAllPayrollDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<GetAllPayrollDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = payroll
            });
        }

        [HttpGet("{get-payroll-overtime-byguid}")]
        public IActionResult GetpayrollOver(Guid guid)
        {
            var payroll = _payrollService.GetAllMasterOverbyGuid(guid);
            if (!payroll.Any())
            {
                return NotFound(new HandlerForResponse<GetAllPayrollDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<GetAllPayrollDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = payroll
            });
        }

        [HttpGet("get-payroll-overtime-by-employeeguid/{guid}")]
        public IActionResult GetpayrollOverEmp(Guid guid)
        {
            var payroll = _payrollService.GetAllPayrollOverbyEmpGuid(guid);
            if (!payroll.Any())
            {
                return NotFound(new HandlerForResponse<GetAllPayrollDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            return Ok(new HandlerForResponse<IEnumerable<GetAllPayrollDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Data Found",
                Data = payroll
            });
        }

        [HttpGet("total-expense")]
        public IActionResult GetTotalSalaryExpense()
        {
            double totalExpense = _payrollService.GetTotalSalaryExpense();

            return Ok(new HandlerForResponse<double>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Total Salary Expense Calculated",
                Data = totalExpense
            });
        }

        [HttpGet("total-paid-overtime")]
        public IActionResult GetTotalPaidOvertime(Guid guid)
        {
            double totalExpense = _payrollService.GetTotalOvertime(guid);

            return Ok(new HandlerForResponse<double>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Total Salary Expense Calculated",
                Data = totalExpense
            });
        }
    }
}
