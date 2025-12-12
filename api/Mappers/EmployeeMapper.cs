using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Employee;
using api.Models;

namespace api.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeDto ToEmployeeDto(this Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                LastName = employee.LastName,
                Position = employee.Position,
                Notes = employee.Notes.Select(x => x.ToNoteDto()).ToList()
            };
        }

        public static Employee ToEmployeeFromCreateDto(this CreateEmployeeDto employee)
        {
            return new Employee
            {
                Name = employee.Name,
                LastName = employee.LastName,
                Position = employee.Position,
                Salary = employee.Salary
            };
        }
    }
}