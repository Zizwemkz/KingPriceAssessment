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
        public async Task<IEnumerable<Role>> GetAllAsync() => await _context.Roles.ToListAsync();
        public async Task<Role> GetByIdAsync(int id) => await _context.Roles.FindAsync(id);
        public async Task AddAsync(Role role) { _context.Roles.Add(role); await _context.SaveChangesAsync(); }
        public async Task UpdateAsync(Role role) { _context.Roles.Update(role); await _context.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol != null)
            {
                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();
            }
        }
    }
}
