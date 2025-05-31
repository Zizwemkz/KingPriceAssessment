using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Repository;
using KingPriceAssessment.Common.Service;
using KingPriceAssessment.Data.Request.Add;
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

        public async Task<EmployeeAllocation> GetAllocationByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Allocation ID must be greater than 0", nameof(id));
            }

            var allocation = await _allocationRepository.GetByIdAsync(id);
            if (allocation == null)
            {
                throw new KeyNotFoundException($"No Allocation found with ID {id}");
            }
            return allocation;
        }

        public async Task AddAllocationAsync(EmployeeAllocation allocation)
        {
            if (allocation == null)
            {
                throw new ArgumentNullException(nameof(allocation));
            }
           
            var allAllocations = await _allocationRepository.GetAllAsync();
            if (allAllocations.Any(a =>
                a.EmployeeId == allocation.EmployeeId &&
                a.RoleId == allocation.RoleId &&
                a.DepartmentId == allocation.DepartmentId))
            {
                throw new ArgumentException("This allocation already exists for the specified employee, role, and department.");
            }

            await _allocationRepository.AddAsync(allocation);
        }

        public async Task UpdateAllocationAsync(EmployeeAllocation allocation)
        {
            if (allocation == null)
            {
                throw new ArgumentNullException(nameof(allocation));
            }

            var existingAllocation = await _allocationRepository.GetByIdAsync(allocation.Id);
            if (existingAllocation == null)
            {
                throw new KeyNotFoundException($"No Allocation found with ID {allocation.Id}");
            }

            var allAllocations = await _allocationRepository.GetAllAsync();
            if (allAllocations.Any(a =>
                a.EmployeeId == allocation.EmployeeId &&
                a.RoleId == allocation.RoleId &&
                a.DepartmentId == allocation.DepartmentId &&
                a.Id != allocation.Id))
            {
                throw new ArgumentException("This allocation already exists for the specified employee, role, and department.");
            }

            await _allocationRepository.UpdateAsync(allocation);
        }

        public async Task DeleteAllocationAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Allocation ID must be greater than 0", nameof(id));
            }

            var existingAllocation = await _allocationRepository.GetByIdAsync(id);
            if (existingAllocation == null)
            {
                throw new KeyNotFoundException($"No Allocation found with ID {id}");
            }

            await _allocationRepository.DeleteAsync(id);
        }
    }
}