using System;
using System.Threading.Tasks;
using TechMove1._3.Application.Services;
using TechMove1._3.Domain.Entities;
using Xunit;

namespace TechMove1._3.Tests.Services
{
    public class ServiceRequestServiceTests
    {
        [Fact]
        public void ValidateContract_AllowsActiveContracts()
        {
            var svc = new ServiceRequestService();
            var contract = new Contract
            {
                Status = ContractStatus.Active,
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            // should not throw
            svc.ValidateContract(contract);
        }

        [Fact]
        public void ValidateContract_Throws_OnExpiredStatus()
        {
            var svc = new ServiceRequestService();
            var contract = new Contract
            {
                Status = ContractStatus.Expired,
                EndDate = DateTime.UtcNow.AddDays(-1)
            };

            Assert.Throws<InvalidOperationException>(() => svc.ValidateContract(contract));
        }

        [Fact]
        public void ValidateContract_Throws_OnOnHold()
        {
            var svc = new ServiceRequestService();
            var contract = new Contract
            {
                Status = ContractStatus.OnHold,
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            Assert.Throws<InvalidOperationException>(() => svc.ValidateContract(contract));
        }
    }
}
