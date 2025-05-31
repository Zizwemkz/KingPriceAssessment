using KingPriceAssessment.Common.Repository;
using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public RoleRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                return await _employeeDbContext.Roles.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving roles.", ex);
            }
        }
        public async Task<Role> GetByIdAsync(int id)
        {
            try
            {
                var role = await _employeeDbContext.Roles.FindAsync(id);
                return role;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the role.", ex);
            }
        }
        public async Task AddAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            try
            {
                await _employeeDbContext.Roles.AddAsync(role);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the role.", ex);
            }
        }
        public async Task UpdateAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            try
            {
                _employeeDbContext.Roles.Update(role);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the role.", ex);
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                var rol = await _employeeDbContext.Roles.FindAsync(id);
                _employeeDbContext.Roles.Remove(rol);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the role.", ex);
            }
        }
    }
}
