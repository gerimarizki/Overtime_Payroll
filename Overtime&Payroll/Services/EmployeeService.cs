using server.Contracts;
using server.DTOs.AccountRoles;
using server.DTOs.Accounts;
using server.DTOs.Employees;
using server.Models;
using server.Repositories;
using server.Utilities.Handlers;

namespace server.Services
{
    public class EmployeeService
    {
        private readonly IPayrollRepository _payrollRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public EmployeeService(IPayrollRepository payrollRepository, IEmployeeRepository employeeRepository, IAccountRepository accountRepository, IAccountRoleRepository accountRoleRepository, IRoleRepository roleRepository)
        {
            _payrollRepository = payrollRepository;
            _employeeRepository = employeeRepository;
            _accountRepository = accountRepository;
            _accountRoleRepository = accountRoleRepository;
            _roleRepository = roleRepository;
        }

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<GetAllEmoployeeDto> GetManager()
        {
            var allEmployee = GetAllEmployee();

            var managerEmployees = allEmployee.Where(employee => employee.RoleName == "Manager");

            return managerEmployees;
        }

        public IEnumerable<GetAllEmoployeeDto> GetAllEmployee()
        {
            var payrolls = _payrollRepository.GetAll();
            var master = (from employee in _employeeRepository.GetAll()
                          join account in _accountRepository.GetAll() on employee.Guid equals account.Guid
                          join accountRole in _accountRoleRepository.GetAll() on account.Guid equals accountRole.AccountGuid
                          join role in _roleRepository.GetAll() on accountRole.RoleGuid equals role.Guid
                          select new GetAllEmoployeeDto
                          {
                              Guid = employee.Guid,
                              FullName = employee.FirstName + " " + employee.LastName,
                              NIK = employee.NIK,
                              Email = account.Email,
                              BirthDate = employee.BirthDate,
                              PhoneNumber = employee.PhoneNumber,
                              Gender = employee.Gender,
                              HiringDate = employee.HiringDate,
                              ManagerGuid = employee.ManagerGuid,
                              RoleName = role.Name
                          }).ToList();

            var managerEmployees = master.Where(employee => employee.RoleName == "Manager");

            foreach (var getDataEmployee in master)
            {
                foreach (var manager in managerEmployees)
                {
                    if (getDataEmployee.ManagerGuid == manager.Guid)
                    {
                        getDataEmployee.Manager = $"{manager.NIK} - {manager.FullName}";
                        break;
                    }
                }

                foreach (var payroll in payrolls)
                {
                    if (payroll.EmployeeGuid == getDataEmployee.Guid)
                    {
                        getDataEmployee.Salary = payroll.Salary;
                        break;
                    }

                }
            }



            return master;
        }

        public IEnumerable<GetEmployeeDto> GetEmployee()
        {
            var employees = _employeeRepository.GetAll().ToList();
            if (!employees.Any()) return Enumerable.Empty<GetEmployeeDto>();
            List<GetEmployeeDto> employeeDtos = new();

            foreach (var employee in employees)
            {
                employeeDtos.Add((GetEmployeeDto)employee);
            }

            return employeeDtos;
        }

        public GetEmployeeDto? GetEmployeeByGuid(Guid guid)
        {

            var employee = _employeeRepository.GetByGuid(guid);
            var account = _accountRepository.GetByGuid(employee.Guid);
            var employeeDtoGet = new GetEmployeeDto();

            employeeDtoGet.Guid = employee.Guid;
            employeeDtoGet.NIK = employee.NIK;
            employeeDtoGet.FirstName = employee.FirstName;
            employeeDtoGet.LastName = employee.LastName;
            employeeDtoGet.BirthDate = employee.BirthDate;
            employeeDtoGet.Gender = employee.Gender;
            employeeDtoGet.HiringDate = employee.HiringDate;
            employeeDtoGet.Email = account.Email;
            employeeDtoGet.PhoneNumber = employee.PhoneNumber;
            employeeDtoGet.ManagerGuid = employee.ManagerGuid;
            if (employee is null) return null;

            return employeeDtoGet;
        }

        public GetEmployeeDto? GetManagerByGuid(Guid guid)
        {
            var manager = _employeeRepository.GetManagerByGuid(guid);
            if (manager is null) return null;

            return (GetEmployeeDto)manager;
        }

        public GetEmployeeDto? CreateEmployee(CreateEmployeeDto newEmployeeDto)
        {
            Employee employee = newEmployeeDto;
            employee.NIK = GenerateNikByService();

            var createdEmployee = _employeeRepository.Create(employee);
            if (createdEmployee is null) return null; // gagal

            return (GetEmployeeDto)createdEmployee; // berhasil
        }

        public int Update(UpdateEmployeeDto employeeDto )
        {
            var getEmployee = _employeeRepository.GetByGuid(employeeDto.Guid);

            if(getEmployee is null) return 0;
            Employee update = employeeDto;
            //update.NIK = getEmployee.NIK;
            update.CreatedDate = getEmployee.CreatedDate; 
            var updateEmployee = _employeeRepository.Update(update);
            return updateEmployee? 1 : -1;
        }

        public int UpdateEmployee(UpdateAllEmployeeDto employeeDto)
        {
            var getEmployee = _employeeRepository.GetByGuid(employeeDto.Guid);

            if (getEmployee is null) return -1; // 404
            var account = _accountRepository.GetByGuid(getEmployee.Guid);
            var accountDtoUpdate = new UpdateAccountDto();
            if (account is null) return -1;

            accountDtoUpdate.Guid = account.Guid;
            accountDtoUpdate.Email = employeeDto.Email;
            accountDtoUpdate.Password = account.Password;
            accountDtoUpdate.IsActive = account.IsActive;
            accountDtoUpdate.OTP = account.OTP;
            accountDtoUpdate.IsUsed = account.IsUsed;
            accountDtoUpdate.ExpiredTime = account.ExpiredTime;

            var accountIsUpdated = _accountRepository.Update(accountDtoUpdate);

            var accountRole = _accountRoleRepository.GetAccountRoles(account.Guid);
            var accountRoleUpdate = new UpdateAccountRoleDto();
            if (accountRole is null) return -1;

            accountRoleUpdate.Guid = accountRole.Guid;
            accountRoleUpdate.AccountGuid = accountRole.AccountGuid;
            accountRoleUpdate.RoleGuid = employeeDto.RoleGuid;

            var accountRoleIsUpdate = _accountRoleRepository.Update(accountRoleUpdate);

            var isUpdate = _employeeRepository.Update(employeeDto);
            return !isUpdate ? 0 : // gagal update
                1; // berhasil
        }

        public int DeleteEmployee(Guid guid)
        {
            var employee = _employeeRepository.GetByGuid(guid);

            if (employee is null) return -1; // 404

            var isDelete = _employeeRepository.Delete(employee);
            return !isDelete ? 0 : // gagal apus
                1; // berhasil
        }

        public int GetTotalEmployeeCount()
        {
            var employees = GetAllEmployee();
            int totalEmployeeCount = employees.Count(employee => employee.RoleName != "Finance");

            return totalEmployeeCount;
        }

        //generate nik by employee service
        public string GenerateNikByService()
        {
            int Nik = 111111;
            var employee = GetEmployee();
            if (employee is null)
            {
                Convert.ToString(Nik);
                return Convert.ToString(Nik);
            }
            var lastEmployee = employee.OrderByDescending(e => e.NIK).FirstOrDefault();
            int newNik = Int32.Parse(lastEmployee.NIK) + 1;
            string lastNik = Convert.ToString(newNik);
            return lastNik;
        }

    }
}
