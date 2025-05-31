using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Common.Service
{
    public interface IEmployeeAllocationService
    {
        public Task<IEnumerable<EmployeeAllocation>> GetAllAllocationsAsync();
        Task<EmployeeAllocation> GetAllocationByIdAsync(int id);
        Task AddAllocationAsync(EmployeeAllocation allocation);
        Task UpdateAllocationAsync(EmployeeAllocation allocation);
        Task DeleteAllocationAsync(int id);
    }
}
