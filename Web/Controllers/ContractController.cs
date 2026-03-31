using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove1._3.Domain.Entities;
using TechMove1._3.Domain.Interfaces;
using TechMove1._3.Infrastructure.Data;

namespace TechMove1._3.Web.Controllers
{
    public class ContractController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public ContractController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var contracts = await _context.Contracts.Include(c => c.Client).ToListAsync();
            return View(contracts);
        }

        public IActionResult Create()
        {
            ViewBag.Clients = _context.Clients.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contract model, IFormFile file)
        {
            if (file != null)
            {
                model.SignedFilePath = await _fileService.SaveFileAsync(file);
            }

            _context.Contracts.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Download(int id)
        {
            var contract = _context.Contracts.Find(id);
            var bytes = System.IO.File.ReadAllBytes(contract.SignedFilePath);
            return File(bytes, "application/pdf", "Agreement.pdf");
        }
    }
}
