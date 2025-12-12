using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Project;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteRepository _noteRepo;
        private readonly IEmployeeRepository _employeeRepo;
        public NoteController(INoteRepository noteRepo, IEmployeeRepository employeeRepo)
        {
            _noteRepo = noteRepo;
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var projects = await _noteRepo.GetAllAsync();
            var projectDto = projects.Select(x => x.ToNoteDto());
            return Ok(projectDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNotesById([FromRoute] int id)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var p = await _noteRepo.GetByIdAsync(id);
            if(p == null)
            {
                return NotFound();
            }
            return Ok(p.ToNoteDto());
        }

        [HttpPost("{employeeId:int}")]
        public async Task<IActionResult> CreateNote([FromRoute] int employeeId, [FromBody] CreateNoteDto noteDto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            if(!await _employeeRepo.EmployeeExists(employeeId))
            {
                return BadRequest("Employee doesn't exist!");
            }
            var note = noteDto.ToNoteFromCreate(employeeId);
            note.EmployeeId = employeeId;
            await _noteRepo.CreateAsync(note);
            return CreatedAtAction(nameof(GetNotesById), new {id = note.Id}, note.ToNoteDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateNote([FromRoute] int id, [FromBody] UpdateNoteDto noteDto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var n = await _noteRepo.UpdateAsync(id, noteDto);
            if(n == null) return NotFound();

            return Ok(n);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
                
            var n = await _noteRepo.DeleteAsync(id);
            if(n == null) return NotFound();
            return NoContent();
        }
    }
}