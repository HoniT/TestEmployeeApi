using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Employee;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace api.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync(EmployeeQuery query)
        {
            var employees = _context.Employees.Include(c => c.Notes).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.Name))
            {
                employees = employees.Where(x => x.Name == query.Name);
            }
            if(!string.IsNullOrWhiteSpace(query.LastName))
            {
                employees = employees.Where(x => x.LastName == query.LastName);
            }
            if(!string.IsNullOrWhiteSpace(query.Position))
            {
                employees = employees.Where(x => x.Position == query.Position);
            }

            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    employees = query.IsDecsending ? employees.OrderByDescending(s => s.Name) :
                        employees.OrderBy(s => s.Name);
                }
                if(query.SortBy.Equals("LastName", StringComparison.OrdinalIgnoreCase))
                {
                    employees = query.IsDecsending ? employees.OrderByDescending(s => s.LastName) :
                        employees.OrderBy(s => s.LastName);
                }
                if(query.SortBy.Equals("Position", StringComparison.OrdinalIgnoreCase))
                {
                    employees = query.IsDecsending ? employees.OrderByDescending(s => s.Position) :
                        employees.OrderBy(s => s.Position);
                }
            }

            var skipNum = (query.PageNumber - 1) * query.PageSize;

            return await employees.Skip(skipNum).Take(query.PageSize).ToListAsync();
        }

        public async ValueTask<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.Include(c => c.Notes).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Employee?> AddAsync(Employee employeeModel)
        {
            await _context.Employees.AddAsync(employeeModel);
            await _context.SaveChangesAsync();
            return employeeModel;
        }

        public async Task<UpdateEmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto employeeDto)
        {
            var employeeModel = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if(employeeModel == null)
            {
                return null;
            }
            
            employeeModel.Name = employeeDto.Name;
            employeeModel.LastName = employeeDto.LastName;
            employeeModel.Position = employeeDto.Position;
            employeeModel.Salary = employeeDto.Salary;
        
            await _context.SaveChangesAsync();
            return employeeDto;
        }

        public async Task<Employee?> DeleteAsync(int id)
        {
            var employeeModel = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if(employeeModel == null)
            {
                return null;
            }

            _context.Employees.Remove(employeeModel);
            await _context.SaveChangesAsync();

            return employeeModel;
        }


        public async Task<List<Employee>> GetAllFromProjectsAsync()
        {
            return await _context.Employees.Include(x => x.Notes).ToListAsync();
        } 

        public async Task<bool> EmployeeExists(int id)
        {
            return await _context.Employees.AnyAsync(x => x.Id == id);
        }
    }
}