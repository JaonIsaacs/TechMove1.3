using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechMove1._3.Application.Factories;
using TechMove1._3.Application.Services;
using TechMove1._3.Domain.Entities;
using TechMove1._3.Domain.Interfaces;

namespace TechMove1._3.Web.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly ServiceRequestService _service;
        private readonly ICurrencyStrategy _currency;
        private readonly IContractRepository _contractRepo;
        private readonly IServiceRequestRepository _srRepo;

        public ServiceRequestController(
            ServiceRequestService service,
            ICurrencyStrategy currency,
            IContractRepository contractRepo,
            IServiceRequestRepository srRepo)
        {
            _service = service;
            _currency = currency;
            _contractRepo = contractRepo;
            _srRepo = srRepo;
        }

        public IActionResult Create()
        {
            ViewBag.Contracts = _contractRepo.QueryWithClient().ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int contractId, string description, decimal usdAmount)
        {
            var contract = await _contractRepo.FindAsync(contractId);
            if (contract == null) return NotFound();

            _service.ValidateContract(contract);

            var zar = await _currency.ConvertToZAR(usdAmount);

            var request = ServiceRequestFactory.Create(contractId, description, zar);

            await _srRepo.AddAsync(request);
            await _srRepo.SaveChangesAsync();

            return RedirectToAction("Index", "Contract");
        }
    }
}
