using Overtime_Payroll.Models;

namespace Overtime_Payroll.DTOs.Roles
{
    public class CreateRoleDto
    {
        public string Name { get; set; }

        public static implicit operator Role(CreateRoleDto newRoleDto)
        {
            return new()
            {
                Name = newRoleDto.Name
            };
        }

        public static explicit operator CreateRoleDto(Role role)
        {
            return new()
            {
                Name = role.Name
            };
        }
    }
}
