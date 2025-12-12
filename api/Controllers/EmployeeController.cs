using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Employee;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;
        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees([FromQuery] EmployeeQuery query)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var employees = await _employeeRepo.GetAllAsync(query);
            var employeeDto = employees.Select(s => s.ToEmployeeDto());
            return Ok(employeeDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var employee = await _employeeRepo.GetByIdAsync(id);
            if(employee == null)
            {
                return NotFound();
            }
            return Ok(employee.ToEmployeeDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto employeeDto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
                
            var employeeModel = employeeDto.ToEmployeeFromCreateDto();
            await _employeeRepo.AddAsync(employeeModel);
            return CreatedAtAction(nameof(GetEmployeeById), new {id = employeeModel.Id}, employeeModel.ToEmployeeDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            
            var e = await _employeeRepo.UpdateAsync(id, updateEmployeeDto);
            if(e == null) return NotFound();

            return Ok(e);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var e = await _employeeRepo.DeleteAsync(id);
            if(e == null) return NotFound();
            return NoContent();
        }

    }
}