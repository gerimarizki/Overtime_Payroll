using Overtime_Payroll.Models;

namespace Overtime_Payroll.DTOs.AccountRoles
{
    public class GetAccountRoles
    {
        public Guid Guid { get; set; }

        public Guid AccountGuid { get; set; }

        public Guid RoleGuid { get; set; }

        public static implicit operator AccountRole(GetAccountRoles getAccountRoleDto)
        {
            return new()
            {
                Guid = getAccountRoleDto.Guid,
                AccountGuid = getAccountRoleDto.AccountGuid,
                RoleGuid = getAccountRoleDto.RoleGuid
            };
        }

        public static explicit operator GetAccountRoles(AccountRole accountRole)
        {
            return new()
            {
                Guid = accountRole.Guid,
                AccountGuid = accountRole.AccountGuid,
                RoleGuid = accountRole.RoleGuid
            };
        }
    }
}