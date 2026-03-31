using TechMove1._3.Domain.Entities;

namespace TechMove1._3.Application.Factories
{
    public class ServiceRequestFactory
    {
        public static ServiceRequest Create(int contractId, string desc, decimal cost)
        {
            return new ServiceRequest
            {
                ContractId = contractId,
                Description = desc,
                CostZAR = cost,
                Status = ServiceStatus.Pending
            };
        }
    }
}
