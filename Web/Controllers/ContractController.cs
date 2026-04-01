using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMove1._3.Domain.Entities;
using TechMove1._3.Domain.Interfaces;
using TechMove1._3.Infrastructure.Data;
using TechMove1._3.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace TechMove1._3.Web.Controllers
{
    public class ContractController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        private readonly ContractService _contractService;
        private readonly Application.Observers.ContractSubject _subject;

        public ContractController(
            AppDbContext context,
            IFileService fileService,
            ContractService contractService,
            Application.Observers.ContractSubject subject)
        {
            _context = context;
            _fileService = fileService;
            _contractService = contractService;
            _subject = subject;
        }

        public async Task<IActionResult> Index(DateTime? start, DateTime? end, ContractStatus? status)
        {
            var contracts = await _contractService.Search(start, end, status);
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
            if (!ModelState.IsValid)
                return View(model);

            if (model.StartDate >= model.EndDate)
            {
                ModelState.AddModelError(string.Empty, "Start date must be before end date.");
                ViewBag.Clients = _context.Clients.ToList();
                return View(model);
            }

            if (!Enum.IsDefined(typeof(ContractStatus), model.Status))
                model.Status = ContractStatus.Draft;

            // Save contract first so it gets an Id
            _context.Contracts.Add(model);
            await _context.SaveChangesAsync();

            if (file != null)
            {
                // Save file to disk (FileService validates extension and content)
                var path = await _fileService.SaveFileAsync(file);

                // store legacy path on contract (optional)
                model.SignedFilePath = path;
                _context.Contracts.Update(model);

                // persist metadata record
                var meta = new FileMetadata
                {
                    ContractId = model.Id,
                    FileName = Path.GetFileName(path),
                    Path = path,
                    Size = new FileInfo(path).Length,
                    ContentType = file.ContentType ?? "application/pdf",
                    UploadDate = DateTime.UtcNow
                };
                _context.FileMetadatas.Add(meta);

                await _context.SaveChangesAsync();
            }

            // notify observers about new contract
            _subject.Notify($"Contract {model.Id} created for client {model.ClientId}");

            return RedirectToAction("Index");
        }
            
        public IActionResult Download(int id)
        {
            // find most recent file for contract
            var meta = _context.FileMetadatas
                .Where(f => f.ContractId == id)
                .OrderByDescending(f => f.UploadDate)
                .FirstOrDefault();

            if (meta == null || string.IsNullOrWhiteSpace(meta.Path))
                return NotFound();

            if (!System.IO.File.Exists(meta.Path))
                return NotFound();

            var stream = new FileStream(meta.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, meta.ContentType ?? "application/pdf", meta.FileName);
        }
    }
}
