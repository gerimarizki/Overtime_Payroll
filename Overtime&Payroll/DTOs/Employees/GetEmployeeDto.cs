﻿using Overtime_Payroll.Models;
using Overtime_Payroll.Utilities.Enums;

namespace Overtime_Payroll.DTOs.Employees
{

    public class GetEmployeeDto
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


        public static implicit operator Employee(GetEmployeeDto updateemployeeDto)
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

        public static explicit operator GetEmployeeDto(Employee employee)
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