using TechMove1._3.Domain.Entities;

namespace TechMove1._3.Application.Services
{
    public class ServiceRequestService
    {
        public void ValidateContract(Contract contract)
        {
            if (contract.Status == ContractStatus.Expired ||
                contract.Status == ContractStatus.OnHold)
            {
                throw new Exception("Cannot create service request");
            }
        }
    }
}
