using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Repository;
using KingPriceAssessment.Common.Service;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<IEnumerable<Role>> GetAllRolesAsync()
            => _roleRepository.GetAllAsync();

        public Task<Role> GetRoleByIdAsync(int id)
            => _roleRepository.GetByIdAsync(id);

        public Task AddRoleAsync(Role role)
            => _roleRepository.AddAsync(role);

        public Task UpdateRoleAsync(Role role)
            => _roleRepository.UpdateAsync(role);

        public Task DeleteRoleAsync(int id)
            => _roleRepository.DeleteAsync(id);
    }
}
