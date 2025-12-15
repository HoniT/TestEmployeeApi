using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Models;

namespace api.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", "Files");

        public FileController()
        {
            if (!Directory.Exists(_uploadDir))
                Directory.CreateDirectory(_uploadDir);
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        [Produces("application/json")]
        public async Task<IActionResult> UploadFile([FromForm] FileUpload fileModel)
        {
            if (fileModel?.File == null || fileModel.File.Length == 0)
                return BadRequest("Invalid file");

            var filePath = Path.Combine(_uploadDir, fileModel.File.FileName);
            var dbPath = $"/Resources/Files/{fileModel.File.FileName}";

            if (System.IO.File.Exists(filePath))
                return BadRequest("File already exists: " + dbPath);

            using var stream = new FileStream(filePath, FileMode.Create);
            await fileModel.File.CopyToAsync(stream);

            return Ok(new { dbPath });
        }

        [HttpPost("multiupload"), DisableRequestSizeLimit]
        [Produces("application/json")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded");

            var uploadedFiles = new List<object>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                    continue;

                var filePath = Path.Combine(_uploadDir, file.FileName);

                if (System.IO.File.Exists(filePath))
                    return BadRequest("File already exists: " + file.FileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                var downloadUrl = Url.Action("DownloadFile", "File", new { fileName = file.FileName }, Request.Scheme);
                uploadedFiles.Add(new { fileName = file.FileName, downloadUrl });
            }

            return Ok(new { files = uploadedFiles });
        }


        [HttpGet("download/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            var path = Path.Combine(_uploadDir, fileName);
            if (!System.IO.File.Exists(path))
                return NotFound();

            return PhysicalFile(path, "application/octet-stream", fileName);
        }
    }
}
