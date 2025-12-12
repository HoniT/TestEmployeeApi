using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Project;
using api.Models;

namespace api.Interfaces
{
    public interface INoteRepository
    {
        Task<List<Note>> GetAllAsync();
        Task<Note?> GetByIdAsync(int id);
        Task<Note> CreateAsync(Note note);
        Task<UpdateNoteDto?> UpdateAsync(int id, UpdateNoteDto noteDto);
        Task<Note?> DeleteAsync(int id);
    }
}