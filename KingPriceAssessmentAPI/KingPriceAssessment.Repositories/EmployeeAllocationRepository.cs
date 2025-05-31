using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Repository;
using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Repositories
{
    public class EmployeeAllocationRepository : IEmployeeAllocationRepository
    {
        private readonly EmployeeDbContext EmployeeDbContext;

        public EmployeeAllocationRepository(EmployeeDbContext context)
        {
            EmployeeDbContext = context;
        }

        public async Task<IEnumerable<EmployeeAllocation>> GetAllAsync()
        {
            try
            {
                return await EmployeeDbContext.EmployeeAllocation
                    .Include(a => a.Employee)
                    .Include(a => a.Role)
                    .Include(a => a.Department)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving employee allocations.", ex);
            }
        }

        public async Task<EmployeeAllocation> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("EmployeeAllocation ID must be greater than 0.", nameof(id));

            try
            {
                var allocation = await EmployeeDbContext.EmployeeAllocation
                    .Include(a => a.Employee)
                    .Include(a => a.Role)
                    .Include(a => a.Department)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (allocation == null)
                    throw new KeyNotFoundException($"No EmployeeAllocation found with ID {id}.");

                return allocation;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the employee allocation.", ex);
            }
        }
       

public async Task AddAsync(EmployeeAllocation allocation)
        {
            try
            {
                if (await EmployeeDbContext.EmployeeAllocation.AnyAsync(a =>
                        a.EmployeeId == allocation.EmployeeId &&
                        a.RoleId == allocation.RoleId &&
                        a.DepartmentId == allocation.DepartmentId))
                {
                    throw new InvalidOperationException("This allocation already exists for the specified employee, role, and department.");
                }

                await EmployeeDbContext.EmployeeAllocation.AddAsync(allocation);
                await EmployeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the employee allocation.", ex);
            }
        }

        public async Task UpdateAsync(EmployeeAllocation allocation)
        {
            try
            {
                var existingAllocation = await EmployeeDbContext.EmployeeAllocation.FindAsync(allocation.Id);
                if (existingAllocation == null)
                    throw new KeyNotFoundException($"No EmployeeAllocation found with ID {allocation.Id}.");

                if (await EmployeeDbContext.EmployeeAllocation.AnyAsync(a =>
                        a.EmployeeId == allocation.EmployeeId &&
                        a.RoleId == allocation.RoleId &&
                        a.DepartmentId == allocation.DepartmentId &&
                        a.Id != allocation.Id))
                {
                    throw new InvalidOperationException("This allocation already exists for the specified employee, role, and department.");
                }

                EmployeeDbContext.EmployeeAllocation.Update(allocation);
                await EmployeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the employee allocation.", ex);
            }

           
            await EmployeeDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("EmployeeAllocation ID must be greater than 0.", nameof(id));

            try
            {
                var allocation = await EmployeeDbContext.EmployeeAllocation.FindAsync(id);
                if (allocation == null)
                    throw new KeyNotFoundException($"No EmployeeAllocation found with ID {id}.");

                EmployeeDbContext.EmployeeAllocation.Remove(allocation);
                await EmployeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the employee allocation.", ex);
            }
        }
    }
}
