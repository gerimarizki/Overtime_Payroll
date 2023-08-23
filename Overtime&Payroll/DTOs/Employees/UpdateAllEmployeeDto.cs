using server.Models;
using server.Utilities.Enums;

namespace server.DTOs.Employees
{
    public class UpdateAllEmployeeDto
    {
        public Guid Guid { get; set; }
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public GenderLevel Gender { get; set; }
        public DateTime HiringDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ManagerGuid { get; set; }
        public Guid RoleGuid { get; set; }


        public static implicit operator Employee(UpdateAllEmployeeDto updateemployeeDto)
        {
            return new()
            {
                Guid = updateemployeeDto.Guid,
                NIK = updateemployeeDto.NIK,
                FirstName = updateemployeeDto.FirstName,
                LastName = updateemployeeDto.LastName,
                BirthDate = updateemployeeDto.BirthDate,
                Gender = updateemployeeDto.Gender,
                HiringDate = updateemployeeDto.HiringDate,
                PhoneNumber = updateemployeeDto.PhoneNumber,
                ManagerGuid = updateemployeeDto.ManagerGuid
            };
        }

        public static explicit operator UpdateAllEmployeeDto(Employee employee)
        {
            return new()
            {
                Guid = employee.Guid,
                NIK = employee.NIK,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate,
                PhoneNumber = employee.PhoneNumber,
                ManagerGuid = employee.ManagerGuid
            };
        }
    }
}
