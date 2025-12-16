using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace VideoChunkUpload.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoUploadController : ControllerBase
    {
        private readonly string _chunkFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resources", "Videos");

        public VideoUploadController()
        {
            if (!Directory.Exists(_chunkFolder))
                Directory.CreateDirectory(_chunkFolder);
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload([FromForm] FileUpload upload)
        {
            if (upload.File == null || upload.File.Length == 0)
                return BadRequest("No file uploaded.");
            var originalFileName = Path.GetFileName(upload.File.FileName);
            var chunkSize = 5 * 1024 * 1024; // 5MB
            var totalChunks = (int)Math.Ceiling((double)upload.File.Length / chunkSize);

            using (var stream = upload.File.OpenReadStream())
            {
                for (int i = 0; i < totalChunks; i++)
                {
                    var chunkPath = Path.Combine(_chunkFolder, $"{originalFileName}.part{i}");
                    using (var chunkStream = new FileStream(chunkPath, FileMode.Create))
                    {
                        var buffer = new byte[chunkSize];
                        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        await chunkStream.WriteAsync(buffer, 0, bytesRead);
                    }
                }
            }

            return Ok(new
            {
                Message = "File uploaded and split into chunks successfully",
                TotalChunks = totalChunks
            });
        }

        [HttpGet("download-full")]
        public IActionResult DownloadFull(string fileName)
        {
            var chunkFiles = Directory
                .GetFiles(_chunkFolder, $"{fileName}.part*")
                .OrderBy(f => f)
                .ToList();

            if (!chunkFiles.Any())
                return NotFound("File not found");

            var memoryStream = new MemoryStream();
            foreach (var chunk in chunkFiles)
            {
                using var chunkStream = new FileStream(chunk, FileMode.Open, FileAccess.Read);
                chunkStream.CopyTo(memoryStream);
            }
            memoryStream.Position = 0;
            return File(memoryStream, "application/octet-stream", fileName);
        }
    }
}
