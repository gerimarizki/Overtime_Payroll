using Microsoft.IdentityModel.Tokens;
using Overtime_Payroll.Contracts;
using Overtime_Payroll.Data;
using Overtime_Payroll.DTOs.Accounts;
using Overtime_Payroll.Models;
using Overtime_Payroll.Utilities.Handlers;
using System.Security.Claims;

namespace Overtime_Payroll.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;

        private readonly OvertimeDbContext _context;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly IRoleRepository _roleRepository;

        private readonly IAccountRoleRepository _accountRoleRepository;

        private readonly IOvertimeRepository _overtimeRepository;

        private readonly IPayrollRepository _payrollRepository;

        private readonly IEmailHandler _emailHandler;

        private readonly ITokenHandler _tokenHandler;

        public AccountService(IAccountRepository accountRepository, OvertimeDbContext context, IEmployeeRepository employeeRepository, IRoleRepository roleRepository, IAccountRoleRepository accountRoleRepository, IOvertimeRepository overtimeRepository, IEmailHandler emailHandler, ITokenHandler tokenHandler, IPayrollRepository payrollRepository)
        {
            _accountRepository = accountRepository;
            _context = context;
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            _accountRoleRepository = accountRoleRepository;
            _overtimeRepository = overtimeRepository;
            _emailHandler = emailHandler;
            _tokenHandler = tokenHandler;
            _payrollRepository = payrollRepository;
        }

        public bool RegistrationAccount(RegisterAccountDto registerDto)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var NIK = HandlerGenerator.NIK(_employeeRepository.GetLastEmployeeNIK());
                var employeeGuid = Guid.NewGuid();
                var employee = new Employee
                {
                    Guid = employeeGuid,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    BirthDate = registerDto.BirthDate,
                    Gender = registerDto.Gender,
                    NIK = NIK,
                    HiringDate = registerDto.HiringDate,
                    PhoneNumber = registerDto.PhoneNumber,
                    ManagerGuid = registerDto.ManagerGuid,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                _employeeRepository.Create(employee);

                var account = new Account
                {
                    Guid = employee.Guid,
                    Email = registerDto.Email,
                    Password = HandlerForHashing.Hash(registerDto.Password),
                    IsActive = false,
                    IsUsed = false,
                    OTP = 0,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    ExpiredTime = DateTime.Now.AddMinutes(10)
                };
                _accountRepository.Create(account);

                var payrollGuid = Guid.NewGuid();
                var payroll = new Payroll
                {
                    Guid = payrollGuid,
                    Salary = registerDto.Salary,
                    Allowance = registerDto.Salary * 3 / 100,
                    EmployeeGuid = employee.Guid
                };
                _payrollRepository.Create(payroll);

                var roleId = Guid.Parse("d45a2669-59da-4998-69f2-08db92586f59");

                var accountRoleGuid = Guid.NewGuid();//untuk nampung Guid di Variabel
                var accountRole = new AccountRole
                {
                    Guid = accountRoleGuid,
                    AccountGuid = account.Guid,
                    RoleGuid = roleId
                };
                _accountRoleRepository.Create(accountRole);

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public string LoginAccount(LoginAccountDto login)
        {
            var account = _accountRepository.GetEmployeeByEmail(login.Email);
            if (account == null) return "0";
            var employee = _employeeRepository.GetByGuid(account.Guid);
            var overtime = _overtimeRepository.RemainingOvertimeByEmployeeGuid(account.Guid);
            var manager = employee.ManagerGuid != null ? _employeeRepository.GetByGuid(employee.ManagerGuid.Value) : null;

            if (!HandlerForHashing.Validate(login.Password, account!.Password)) return "-1";

            var employees = _employeeRepository.GetByGuid(account.Guid);
            try
            {
                var claims = new List<Claim>() {
                new Claim("Guid", employee.Guid.ToString()),
                new Claim("NIK", employees.NIK),
                new Claim("FullName", $"{employees.FirstName} {employees.LastName}"),
                new Claim("EmailAddress", login.Email),
                new Claim("OvertimeRemaining", overtime.OvertimeRemaining.ToString()),
                new Claim("Manager", manager != null ? $"{manager.FirstName} {manager.LastName}" : "N/A")
            };

                var AccountRole = _accountRoleRepository.GetAccountRolesByAccountGuid(account.Guid);
                var getRoleNameByAccountRole = from ar in AccountRole
                                               join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                                               select r.Name;

                claims.AddRange(getRoleNameByAccountRole.Select(role => new Claim(ClaimTypes.Role, role)));

                var getToken = _tokenHandler.GenerateToken(claims);
                return getToken;
            }
            catch
            {
                return "-2";
            }
        }

        // Forgot Password
        public int ForgotPasswordAccountDto(ForgotPasswordAccountDto forgotPasswordDto)
        {
            var getAccountDetail = (from a in _accountRepository.GetAll()
                                    where a.Email == forgotPasswordDto.Email
                                    select a).FirstOrDefault();
            _accountRepository.Clear();

            if (getAccountDetail is null)
            {
                return 0;
            }

            var otp = new Random().Next(111111, 999999);
            var account = new Account
            {
                Guid = getAccountDetail.Guid,
                Password = getAccountDetail.Password,
                ExpiredTime = DateTime.Now.AddMinutes(5),
                OTP = otp,
                IsUsed = false,
                IsActive = false,
                Email = getAccountDetail.Email,
                CreatedDate = getAccountDetail.CreatedDate,
                ModifiedDate = DateTime.Now
            };

            var isUpdated = _accountRepository.Update(account);
            if (!isUpdated)
            {
                return -1;
            }

            _emailHandler.SendEmail(forgotPasswordDto.Email, "Overtime - Forgot Password", $"Your Otp is {otp}");

            return 1;
        }


        // Change Password
        public int ChangePassword(ChangePasswordAccountDto changePasswordDto)
        {
            var account = _accountRepository.GetEmployeeByEmail(changePasswordDto.Email);
            if (account is null)
            {
                return 0;
            }

            if (account.IsUsed)
            {
                return -1;
            }

            if (account.OTP != changePasswordDto.OTP)
            {
                return -2;
            }

            if (account.ExpiredTime < DateTime.Now)
            {
                return -3;
            }
            _accountRepository.Clear();//perlu clear untuk change password

            var updatePassword = new Account
            {
                Guid = account.Guid,
                Email = account.Email,
                Password = HandlerForHashing.Hash(changePasswordDto.NewPassword),
                IsActive = account.IsActive,
                OTP = account.OTP,
                ExpiredTime = account.ExpiredTime,
                IsUsed = false,
                CreatedDate = account.CreatedDate,
                ModifiedDate = DateTime.Now
            };
            var isUpdated = _accountRepository.Update(updatePassword);

            return isUpdated ? 1 : -4;
        }

        public IEnumerable<GetAccountDto> GetAccount()
        {
            var accounts = _accountRepository.GetAll().ToList();
            if (!accounts.Any()) return Enumerable.Empty<GetAccountDto>();
            List<GetAccountDto> accountDtos = new();

            foreach (var account in accounts)
            {
                accountDtos.Add((GetAccountDto)account);
            }

            return accountDtos;
        }

        public GetAccountDto? GetAccountByGuid(Guid guid)
        {
            var account = _accountRepository.GetByGuid(guid);
            if (account is null) return null;

            return (GetAccountDto)account;
        }

        public GetAccountDto? CreateAccount(CreateAccountDto newAccountDto)
        {
            var createdAccount = _accountRepository.Create(newAccountDto);
            if (createdAccount is null) return null;

            return (GetAccountDto)createdAccount;
        }

        public int UpdateAccount(UpdateAccountDto updateAccountDto)
        {
            var getAccount = _accountRepository.GetByGuid(updateAccountDto.Guid);

            if (getAccount is null) return -1;

            var isUpdate = _accountRepository.Update(updateAccountDto);
            return !isUpdate ? 0 :
                1;
        }

        public int DeleteAccount(Guid guid)
        {
            var account = _accountRepository.GetByGuid(guid);

            if (account is null) return -1;

            var isDelete = _accountRepository.Delete(account);
            return !isDelete ? 0 :
                1;
        }
    }
}
