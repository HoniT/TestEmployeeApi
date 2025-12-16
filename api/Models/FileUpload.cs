using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class FileUpload
    {
        [Required]
        public IFormFile File { get; set; }
    }
}