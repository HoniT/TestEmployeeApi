using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Project;

namespace api.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        public List<Note> Notes { get ; set; } = new List<Note>();
    }
}