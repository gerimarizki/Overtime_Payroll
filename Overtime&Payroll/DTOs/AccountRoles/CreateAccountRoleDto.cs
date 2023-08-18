using Overtime_Payroll.Models;

namespace Overtime_Payroll.DTOs.AccountRoles
{
    public class CreateAccountRoleDto
    {
        public Guid AccountGuid { get; set; }

        public Guid RoleGuid { get; set; }

        public static implicit operator AccountRole(CreateAccountRoleDto newAccountRoleDto)
        {
            return new()
            {
                Guid = new Guid(),
                AccountGuid = newAccountRoleDto.AccountGuid,
                RoleGuid = newAccountRoleDto.RoleGuid
            };
        }

        public static explicit operator CreateAccountRoleDto(AccountRole accountRole)
        {
            return new()
            {
                AccountGuid = accountRole.AccountGuid,
                RoleGuid = accountRole.RoleGuid
            };
        }
    }
}
