using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Project
{
    public class UpdateNoteDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Please enter a note with 5+ characters")]
        public string NoteData { get; set; } = string.Empty;
    }
}