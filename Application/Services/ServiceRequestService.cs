using System;
using TechMove1._3.Domain.Entities;

namespace TechMove1._3.Application.Services
{
    public class ServiceRequestService
    {
        public void ValidateContract(Contract contract)
        {
            if (contract == null) throw new ArgumentNullException(nameof(contract));

            if (contract.Status == ContractStatus.Expired ||
                contract.Status == ContractStatus.OnHold)
            {
                throw new InvalidOperationException("Cannot create service request for expired or on-hold contracts.");
            }

            if (contract.EndDate < DateTime.UtcNow)
                throw new InvalidOperationException("Cannot create service request for expired contract (end date passed).");
        }
    }
}
