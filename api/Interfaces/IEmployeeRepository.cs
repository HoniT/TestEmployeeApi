using api.Dtos.Employee;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync(EmployeeQuery query);
        ValueTask<Employee?> GetByIdAsync(int id);
        Task<Employee?> AddAsync(Employee employeeModel);
        Task<UpdateEmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto employeeDto);
        Task<Employee?> DeleteAsync(int id);

        Task<bool> EmployeeExists(int id);
    }
}