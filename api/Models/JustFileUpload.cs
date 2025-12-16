using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class JustFileUpload
    {
        public List<IFormFile> Files { get; set; } = new();
    }
}