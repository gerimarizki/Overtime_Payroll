using Microsoft.IdentityModel.Tokens;
using server.Contracts;
using server.Data;
using server.DTOs.Accounts;
using server.DTOs.Payrolls;
using server.Models;
using server.Repositories;
using server.Utilities.Enums;
using server.Utilities.Handlers;
using System.Security.Claims;

namespace server.Services
{
    //Account Service By Ahlul
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

        private readonly PayrollService _payrollService;

        public AccountService(IAccountRepository accountRepository, OvertimeDbContext context, IEmployeeRepository employeeRepository, IRoleRepository roleRepository, IAccountRoleRepository accountRoleRepository, IOvertimeRepository overtimeRepository, IEmailHandler emailHandler, ITokenHandler tokenHandler, IPayrollRepository payrollRepository, PayrollService payrollService)
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
            _payrollService = payrollService;
        }

        public bool RegistrationAccount(RegisterAccountDto registerDto)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                //fix NIK bug stuck 111113 infinite
                var employeeService = new EmployeeService(_employeeRepository);

                var employeeGuid = Guid.NewGuid();
                var employee = new Employee
                {
                    Guid = employeeGuid,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    BirthDate = registerDto.BirthDate,
                    Gender = registerDto.Gender,
                    NIK = employeeService.GenerateNikByService(),
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

                var roleId = Guid.Parse("34fccb76-f14f-41c5-a07a-08dba403d7e5");

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

        public IEnumerable<GetDetailProfilDto>? GetAllDetailProfil(Guid guid)
        {

            /*var payover = _payrollService.GetAllPayrollOverbyEmpGuid(guid);*/
            var profil = (from p in _payrollRepository.GetAll()
                          join e in _employeeRepository.GetAll() on p.EmployeeGuid equals e.Guid
                          join o in _overtimeRepository.GetAll() on e.Guid equals o.EmployeeGuid
                          join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                          join ar in _accountRoleRepository.GetAll() on a.Guid equals ar.AccountGuid
                          join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                          /*join pay in payover on p.EmployeeGuid equals pay.EmployeeGuid*/
                          where e.Guid == guid
                          select new GetDetailProfilDto
                          {
                              FullName = e.FirstName + " " + e.LastName,
                              Email = a.Email,
                              Nik = e.NIK,
                              PhoneNumber = e.PhoneNumber,
                              RoleName = r.Name,
                              Allowance = p.Allowance,
                              Salary = p.Salary,
                              //                   OvertimeRemaining = o.OvertimeRemaining,
                              //                   TotalSalary = pay.Salary + pay.PaidOvertime - p.Allowance,
                              //               });

                              //return profil.LastOrDefault();

                              OvertimeRemaining = o.OvertimeRemaining,
                              PaidOvertime = o.PaidOvertime, // Nilai awal Paid Overtime                             
                              TotalSalary = p.Salary - p.Allowance, // Nilai awal Total Salary
                              Status = o.Status,
                              OvertimeId = o.OvertimeId
                          });
            return profil;
        }
        public GetDetailProfilDto? GetDetailProfil(Guid guid)
        {

            /*var payover = _payrollService.GetAllPayrollOverbyEmpGuid(guid);*/
            var profil = from p in _payrollRepository.GetAll()
                         join e in _employeeRepository.GetAll() on p.EmployeeGuid equals e.Guid
                         join o in _overtimeRepository.GetAll() on e.Guid equals o.EmployeeGuid
                         join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                         join ar in _accountRoleRepository.GetAll() on a.Guid equals ar.AccountGuid
                         join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                         /*join pay in payover on p.EmployeeGuid equals pay.EmployeeGuid*/
                         where e.Guid == guid
                         select new GetDetailProfilDto
                         {
                             FullName = e.FirstName + " " + e.LastName,
                             Email = a.Email,
                             Nik = e.NIK,
                             PhoneNumber = e.PhoneNumber,
                             RoleName = r.Name,
                             Allowance = p.Allowance,
                             Salary = p.Salary,
                             OvertimeRemaining = o.OvertimeRemaining,
                             PaidOvertime = o.PaidOvertime, // Nilai awal Paid Overtime                             
                             TotalSalary = p.Salary - p.Allowance, // Nilai awal Total Salary
                             Status = o.Status,
                             OvertimeId = o.OvertimeId,
                             Counter = 0
                         };
            var newovertime = new GetDetailProfilDto();
            newovertime = profil.LastOrDefault();
            var counterprofil = profil.Count();
            newovertime.Counter = counterprofil;

            //var newovertime = new GetDetailProfilDto();
            //newovertime = profil.Where(lp => lp.EmployeeGuid.Equals(guid)).LastOrDefault();
            //newovertime = profil.LastOrDefault();
            var newprofil = profil.OrderByDescending(profil => profil.OvertimeRemaining).LastOrDefault();
            newprofil.PaidOvertime = 0;
            newprofil.OvertimeRemaining = 0;
            foreach (var item in profil)
            {
                if (newovertime.Counter > 1)
                {
                    newprofil.PaidOvertime += item.PaidOvertime;
                }
                if (newovertime.Counter == 2)
                {
                    newprofil.OvertimeRemaining = item.OvertimeRemaining;
                }
                newovertime.Counter--;

            }
                /*var overtime = _overtimeRepository.GetOvertimeByOvertimeId(item.OvertimeId);
                if (overtime == null && overtime.Status == StatusLevel.Waiting)
                {
                    newovertime.PaidOvertime = 0;
                }
                else if (overtime.Status == StatusLevel.Accepted)
                {
                    newovertime.PaidOvertime += item.PaidOvertime;
                }*/
                newovertime.Counter = counterprofil;
                foreach (var item2 in profil)
                {
                    if (newovertime.Status == StatusLevel.Waiting)
                    {
                        newovertime = newprofil;
                        break;
                    }
                    else if (newovertime.Status == StatusLevel.Accepted && newovertime.Counter > 1)
                    {
                        newovertime.PaidOvertime += item2.PaidOvertime;
                    }
                    newovertime.Counter--;
                }
                newovertime.TotalSalary = newovertime.Salary + newovertime.PaidOvertime - newovertime.Allowance;
                return newovertime;

            }
        //newovertime.TotalSalary = newovertime.Salary + newovertime.PaidOvertime - newovertime.Allowance;
        //return newovertime;
        //var lastProfil = profil.LastOrDefault();
        //var newovertime = new GetDetailProfilDto();
        //newovertime = lastProfil;
        //foreach (var item in profil)
        //{
        //    var overtime = _overtimeRepository.GetOvertimeByOvertimeId(item.OvertimeId);
        //    if(overtime == null && overtime.Status == StatusLevel.Waiting)
        //    {
        //        newovertime.PaidOvertime = 0;
        //    }
        //    else if(overtime.Status == StatusLevel.Accepted)
        //    {
        //        newovertime.PaidOvertime += item.PaidOvertime;
        //    }

        //}
        //newovertime.TotalSalary = newovertime.Salary + newovertime.PaidOvertime - newovertime.Allowance;
        //return newovertime;
        /* // Cek status Paid Overtime
         if (lastProfil != null && lastProfil.Status  == StatusLevel.Waiting && lastProfil.OvertimeId != null)
         {
             // Jika status "Waiting", maka Paid Overtime tetap 0
             lastProfil.PaidOvertime = 0;
         }
         else if (lastProfil != null && lastProfil.Status == StatusLevel.Accepted )
         {
             // Jika status "Accepted", maka ambil nilai Paid Overtime dari data
             lastProfil.PaidOvertime = payover.LastOrDefault().PaidOvertime;
         }

         // Hitung Total Salary
         lastProfil.TotalSalary = lastProfil.Salary + lastProfil.PaidOvertime - lastProfil.Allowance;*/


        /*
                    return lastProfil;*/
    }

}
