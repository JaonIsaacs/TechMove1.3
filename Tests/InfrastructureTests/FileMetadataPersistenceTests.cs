using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TechMove1._3.Domain.Entities;
using TechMove1._3.Infrastructure.Data;
using TechMove1._3.Infrastructure.Services;
using Xunit;

namespace TechMove1._3.Tests.InfrastructureTests
{
    public class FileMetadataPersistenceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly FileService _fileService;
        private readonly string _dbName = Guid.NewGuid().ToString();

        public FileMetadataPersistenceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(_dbName)
                .Options;

            _context = new AppDbContext(options);
            _fileService = new FileService();
        }

        [Fact]
        public async Task UploadPdf_PersistsFileMetadata()
        {
            // arrange - create contract
            var contract = new Contract
            {
                ClientId = 1,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                Status = ContractStatus.Draft
            };
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            // create a minimal PDF stream ("%PDF")
            var pdfBytes = System.Text.Encoding.ASCII.GetBytes("%PDF-1.4\n%âãÏÓ\n1 0 obj\n<<\n>>\nendobj\n");
            using var ms = new MemoryStream(pdfBytes);
            IFormFile formFile = new FormFile(ms, 0, ms.Length, "file", "agreement.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            // act - save to disk
            var path = await _fileService.SaveFileAsync(formFile);
            Assert.False(string.IsNullOrWhiteSpace(path));
            Assert.True(File.Exists(path));

            // persist metadata
            var meta = new FileMetadata
            {
                ContractId = contract.Id,
                FileName = Path.GetFileName(path),
                Path = path,
                Size = new FileInfo(path).Length,
                ContentType = formFile.ContentType,
                UploadDate = DateTime.UtcNow
            };

            _context.FileMetadatas.Add(meta);
            await _context.SaveChangesAsync();

            // assert
            var persisted = await _context.FileMetadatas.FirstOrDefaultAsync(f => f.ContractId == contract.Id);
            Assert.NotNull(persisted);
            Assert.Equal(meta.FileName, persisted.FileName);
            Assert.Equal(meta.Path, persisted.Path);
        }

        public void Dispose()
        {
            // cleanup any files created by FileService in project root FileStorage
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

            _context?.Dispose();
        }
    }
}
