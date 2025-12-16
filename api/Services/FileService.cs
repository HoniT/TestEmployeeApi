using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace api.Services
{
    public class FileService : IFileService
{
    private const string UploadDir = "Files";
    private const int BufferSize = 81920;

    private readonly string[] _allowedExtensions = [".png", ".jpg"];

    public async Task<FileUploadSummary> UploadFilesAsync(Stream fileStream, string contentType)
    {
        int fileCount = 0;
        long totalSizeInBytes = 0;

        var boundary = GetBoundary(MediaTypeHeaderValue.Parse(contentType));
        var reader = new MultipartReader(boundary, fileStream);

        var filePaths = new List<string>();
        var notUploadedFiles = new List<string>();

        MultipartSection? section;
        while ((section = await reader.ReadNextSectionAsync()) != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection == null)
                continue;

            var bytesWritten = await SaveFileAsync(fileSection, filePaths, notUploadedFiles);
            if (bytesWritten > 0)
            {
                totalSizeInBytes += bytesWritten;
                fileCount++;
            }
        }

        return new FileUploadSummary
        {
            TotalFilesUploaded = fileCount,
            TotalSizeUploaded = ConvertSizeToString(totalSizeInBytes),
            FilePaths = filePaths,
            NotUploadedFiles = notUploadedFiles
        };
    }

    private async Task<long> SaveFileAsync(
        FileMultipartSection fileSection,
        List<string> filePaths,
        List<string> notUploadedFiles)
    {
        var extension = Path.GetExtension(fileSection.FileName);
        if (!_allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            notUploadedFiles.Add(fileSection.FileName);
            return 0;
        }

        Directory.CreateDirectory(UploadDir);

        var safeFileName = Path.GetFileName(fileSection.FileName);
        var filePath = Path.Combine(UploadDir, safeFileName);

        long totalBytes = 0;

        await using var targetStream = new FileStream(
            filePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            BufferSize,
            useAsync: true);

        var buffer = new byte[BufferSize];
        int read;
        while ((read = await fileSection.FileStream.ReadAsync(buffer)) > 0)
        {
            await targetStream.WriteAsync(buffer.AsMemory(0, read));
            totalBytes += read;
        }

        filePaths.Add(Path.GetFullPath(filePath));
        return totalBytes;
    }

    private static string ConvertSizeToString(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }

    private static string GetBoundary(MediaTypeHeaderValue mediaType)
    {
        var boundary = HeaderUtilities.RemoveQuotes(mediaType.Boundary).Value;
        if (string.IsNullOrWhiteSpace(boundary))
            throw new InvalidDataException("Missing content-type boundary");

        return boundary;
    }
}
}