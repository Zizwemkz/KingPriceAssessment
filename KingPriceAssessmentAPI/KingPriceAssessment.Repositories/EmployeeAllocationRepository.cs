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
        private readonly EmployeeDbContext _employeeDbContext;

        public EmployeeAllocationRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public async Task<IEnumerable<EmployeeAllocation>> GetAllAsync()
        {
            try
            {
                return await _employeeDbContext.EmployeeAllocation
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
            try
            {
                var allocation = await _employeeDbContext.EmployeeAllocation
                    .Include(a => a.Employee)
                    .Include(a => a.Role)
                    .Include(a => a.Department)
                    .FirstOrDefaultAsync(a => a.Id == id);

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
                if (await _employeeDbContext.EmployeeAllocation.AnyAsync(a =>
                        a.EmployeeId == allocation.EmployeeId &&
                        a.RoleId == allocation.RoleId &&
                        a.DepartmentId == allocation.DepartmentId))
                {
                    throw new InvalidOperationException("This allocation already exists for the specified employee, role, and department.");
                }

                await _employeeDbContext.EmployeeAllocation.AddAsync(allocation);
                await _employeeDbContext.SaveChangesAsync();
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
                if (await _employeeDbContext.EmployeeAllocation.AnyAsync(a =>
                        a.EmployeeId == allocation.EmployeeId &&
                        a.RoleId == allocation.RoleId &&
                        a.DepartmentId == allocation.DepartmentId &&
                        a.Id != allocation.Id))
                {
                    throw new InvalidOperationException("This allocation already exists for the specified employee, role, and department.");
                }

                _employeeDbContext.EmployeeAllocation.Update(allocation);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the employee allocation.", ex);
            }

           
            await _employeeDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var allocation = await _employeeDbContext.EmployeeAllocation.FindAsync(id);
                _employeeDbContext.EmployeeAllocation.Remove(allocation);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the employee allocation.", ex);
            }
        }
    }
}
