using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Project;
using api.Models;

namespace api.Dtos.Employee
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Enter a valid name")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MinLength(1, ErrorMessage = "Enter a valid lastname")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [MinLength(1, ErrorMessage = "Enter a valid position")]
        public string Position { get; set; } = string.Empty;
        public List<NoteDto> Notes { get ; set; } = new List<NoteDto>();
    }
}