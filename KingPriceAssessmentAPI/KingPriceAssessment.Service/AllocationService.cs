using KingPriceAssessment.Common.Repository;
using KingPriceAssessment.Common.Service;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Service
{
    public class AllocationService : IEmployeeAllocationService
    {
        private readonly IEmployeeAllocationRepository _allocationRepository;

        public AllocationService(IEmployeeAllocationRepository allocationRepository)
        {
            _allocationRepository = allocationRepository;
        }

        public Task<IEnumerable<EmployeeAllocation>> GetAllAllocationsAsync()
            => _allocationRepository.GetAllAsync();

        public Task<EmployeeAllocation> GetAllocationByIdAsync(int id)
            => _allocationRepository.GetByIdAsync(id);

        public Task AddAllocationAsync(EmployeeAllocation allocation)
            => _allocationRepository.AddAsync(allocation);

        public Task UpdateAllocationAsync(EmployeeAllocation allocation)
            => _allocationRepository.UpdateAsync(allocation);

        public Task DeleteAllocationAsync(int id)
            => _allocationRepository.DeleteAsync(id);
    }
}
