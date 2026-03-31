using TechMove1._3.Domain.Interfaces;

namespace TechMove1._3.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _folder = "FileStorage";

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                throw new Exception("Only PDF allowed");

            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            var path = Path.Combine(_folder, Guid.NewGuid() + ".pdf");

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return path;
        }
    }
}
