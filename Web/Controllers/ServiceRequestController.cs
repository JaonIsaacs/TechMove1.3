 using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TechMove1._3.Application.Services;
    using TechMove1._3.Domain.Entities;
    using TechMove1._3.Domain.Interfaces;
    using TechMove1._3.Infrastructure.Data;namespace TechMove1._3.Web.Controllers
{

    public class ServiceRequestController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ServiceRequestService _service;
        private readonly ICurrencyStrategy _currency;

        public ServiceRequestController(AppDbContext context,
            ServiceRequestService service,
            ICurrencyStrategy currency)
        {
            _context = context;
            _service = service;
            _currency = currency;
        }

        public IActionResult Create()
        {
            ViewBag.Contracts = _context.Contracts.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int contractId, string description, decimal usdAmount)
        {
            var contract = await _context.Contracts.FindAsync(contractId);

            _service.ValidateContract(contract);

            var zar = await _currency.ConvertToZAR(usdAmount);

            var request = new ServiceRequest
            {
                ContractId = contractId,
                Description = description,
                CostZAR = zar,
                Status = ServiceStatus.Pending
            };

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Contract");
        }
    }
}
