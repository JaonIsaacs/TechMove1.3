using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TechMove1._3.Domain.Interfaces;

namespace TechMove1._3.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _folderName = "FileStorage";
        private readonly long _maxBytes = 5 * 1024 * 1024; // 5 MB

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".pdf")
                throw new InvalidOperationException("Only PDF files are allowed.");

            if (file.Length == 0 || file.Length > _maxBytes)
                throw new InvalidOperationException("File is empty or exceeds the 5MB limit.");

            // Validate PDF magic bytes ("%PDF")
            using (var headerStream = file.OpenReadStream())
            {
                var header = new byte[4];
                var read = await headerStream.ReadAsync(header, 0, header.Length);
                var headerString = System.Text.Encoding.ASCII.GetString(header, 0, read);
                if (!headerString.StartsWith("%PDF", StringComparison.Ordinal))
                    throw new InvalidOperationException("File content does not appear to be a valid PDF.");
            }

            var storeRoot = Path.Combine(Directory.GetCurrentDirectory(), _folderName);
            if (!Directory.Exists(storeRoot))
                Directory.CreateDirectory(storeRoot);

            var fileName = $"{Guid.NewGuid():N}.pdf";
            var fullPath = Path.Combine(storeRoot, fileName);

            using var stream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            await file.CopyToAsync(stream);

            return fullPath;
        }
    }
}
