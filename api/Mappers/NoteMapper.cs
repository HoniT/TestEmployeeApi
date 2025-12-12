using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Dtos.Project;

namespace api.Mappers
{
    public static class NoteMapper
    {
        public static NoteDto ToNoteDto(this Note project)
        {
            return new NoteDto
            {
                Id = project.Id,
                NoteData = project.NoteData,
                EmployeeId = project.EmployeeId
            };
        }

        public static Note ToNoteFromCreate(this CreateNoteDto noteDto, int employeeId)
        {
            return new Note
            {
                NoteData = noteDto.NoteData,
                EmployeeId = employeeId,
            };
        }
    }
}