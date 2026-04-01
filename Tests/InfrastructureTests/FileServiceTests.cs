using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using TechMove1._3.Infrastructure.Services;
using Xunit;

namespace TechMove1._3.Tests.InfrastructureTests
{
    public class FileServiceTests : IDisposable
    {
        private readonly FileService _fileService;

        public FileServiceTests()
        {
            _fileService = new FileService();
        }

        [Fact]
        public async Task SaveFileAsync_RejectsNonPdf()
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes("not a pdf");
            using var ms = new MemoryStream(bytes);
            IFormFile file = new FormFile(ms, 0, ms.Length, "file", "test.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => _fileService.SaveFileAsync(file));
        }

        [Fact]
        public async Task SaveFileAsync_RejectsLargeFile()
        {
            // create >5MB
            var large = new byte[6 * 1024 * 1024];
            using var ms = new MemoryStream(large);
            IFormFile file = new FormFile(ms, 0, ms.Length, "file", "big.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => _fileService.SaveFileAsync(file));
        }

        [Fact]
        public async Task SaveFileAsync_SavesValidPdf()
        {
            var pdfBytes = System.Text.Encoding.ASCII.GetBytes("%PDF-1.4\n%âãÏÓ\n1 0 obj\n<<\n>>\nendobj\n");
            using var ms = new MemoryStream(pdfBytes);
            IFormFile formFile = new FormFile(ms, 0, ms.Length, "file", "agreement.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            var path = await _fileService.SaveFileAsync(formFile);

            Assert.False(string.IsNullOrWhiteSpace(path));
            Assert.True(File.Exists(path));
        }

        public void Dispose()
        {
            try
            {
                var storage = Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");
                if (Directory.Exists(storage))
                {
                    foreach (var f in Directory.GetFiles(storage))
                    {
                        try { File.Delete(f); } catch { }
                    }
                }
            }
            catch { }
        }
    }
}
