using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class FileUploadSummary
    {
        public int TotalFilesUploaded { get; set; }
        public string? TotalSizeUploaded { get; set; }
        public List<string>? FilePaths { get; set; }
        public List<string>? NotUploadedFiles { get; set; }
    }
}