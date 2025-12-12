using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string NoteData { get; set; } = string.Empty;
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}