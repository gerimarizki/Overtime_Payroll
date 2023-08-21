using Overtime_Payroll.Contracts;
using Overtime_Payroll.DTOs.AccountRoles;

namespace Overtime_Payroll.Services
{
    public class AccountRoleService
    {
        private readonly IAccountRoleRepository _accountRoleRepository;

        public AccountRoleService(IAccountRoleRepository accountRoleRepository)
        {
            _accountRoleRepository = accountRoleRepository;
        }

        public IEnumerable<GetAccountRolesDto> GetAccountRole()
        {
            var accountRoles = _accountRoleRepository.GetAll().ToList();
            if (!accountRoles.Any()) return Enumerable.Empty<GetAccountRolesDto>();
            List<GetAccountRolesDto> accountRoleDtos = new();

            foreach (var accountRole in accountRoles)
            {
                accountRoleDtos.Add((GetAccountRolesDto)accountRole);
            }

            return accountRoleDtos;
        }

        public GetAccountRolesDto? GetAccountRoleByGuid(Guid guid)
        {
            var accountRole = _accountRoleRepository.GetByGuid(guid);
            if (accountRole is null) return null;

            return (GetAccountRolesDto)accountRole;
        }

        public GetAccountRolesDto? CreateAccountRole(CreateAccountRoleDto newAccountRoleDto)
        {
            var createdAccountRole = _accountRoleRepository.Create(newAccountRoleDto);
            if (createdAccountRole is null) return null;

            return (GetAccountRolesDto)createdAccountRole;
        }

        public int UpdateAccountRole(UpdateAccountRoleDto updateAccountRoleDto)
        {
            var getAccountRole = _accountRoleRepository.GetByGuid(updateAccountRoleDto.Guid);

            if (getAccountRole is null) return -1;

            var isUpdate = _accountRoleRepository.Update(updateAccountRoleDto);
            return !isUpdate ? 0 :
                1;
        }

        public int DeleteAccountRole(Guid guid)
        {
            var accountRole = _accountRoleRepository.GetByGuid(guid);

            if (accountRole is null) return -1;

            var isDelete = _accountRoleRepository.Delete(accountRole);
            return !isDelete ? 0 :
                1;
        }
    }
}
