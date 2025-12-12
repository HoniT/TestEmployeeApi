using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using api.Dtos.Project;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Note>> GetAllAsync()
        {
            return await _context.Notes.ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            return await _context.Notes.FindAsync(id);
        }

        public async Task<Note> CreateAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<UpdateNoteDto?> UpdateAsync(int id, UpdateNoteDto noteDto)
        {
            var noteModel = await _context.Notes.FirstOrDefaultAsync(x => x.Id == id);
            if(noteModel == null)
            {
                return null;
            }

            noteModel.NoteData = noteDto.NoteData;
        
            await _context.SaveChangesAsync();
            return noteDto;
        }

        public async Task<Note?> DeleteAsync(int id)
        {
            var noteModel = await _context.Notes.FirstOrDefaultAsync(x => x.Id == id);
            if(noteModel == null)
            {
                return null;
            }

            _context.Notes.Remove(noteModel);
            await _context.SaveChangesAsync();

            return noteModel;
        }
    }
}