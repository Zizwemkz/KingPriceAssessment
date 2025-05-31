using KingPriceAssessment.Common.Repository;
using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EmployeeDbContext _context;

        public RoleRepository(EmployeeDbContext EmployeeDb)
        {
            _context = EmployeeDb;
        }
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                return await _context.Roles.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving roles.", ex);
            }
        }
        public async Task<Role> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Role ID must be greater than 0.", nameof(id));

            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    throw new KeyNotFoundException($"No role found with ID {id}.");
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
                if (await _context.Roles.AnyAsync(r => r.RoleName.ToLower() == role.RoleName.ToLower()))
                    throw new InvalidOperationException($"A role with the name '{role.RoleName}' already exists.");

                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
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
                var existingRole = await _context.Roles.FindAsync(role.Id);
                if (existingRole == null)
                    throw new KeyNotFoundException($"No role found with ID {role.Id}.");

                // Prevent duplicate role names (case insensitive), excluding current role
                if (await _context.Roles.AnyAsync(r => r.RoleName.ToLower() == role.RoleName.ToLower() && r.Id != role.Id))
                    throw new InvalidOperationException($"A role with the name '{role.RoleName}' already exists.");

                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Optionally log exception (ex)
                throw new Exception("An error occurred while updating the role.", ex);
            }
        }
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Role ID must be greater than 0.", nameof(id));

            try
            {
                var rol = await _context.Roles.FindAsync(id);
                if (rol == null)
                    throw new KeyNotFoundException($"No role found with ID {id}.");

                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the role.", ex);
            }
        }
    }
}
