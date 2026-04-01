using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TechMove1._3.Domain.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}
