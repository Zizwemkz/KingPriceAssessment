using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Repository;
using KingPriceAssessment.Common.Service;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Role ID cannot be 0 or negative");
            }

            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new KeyNotFoundException($"No Role found with ID {id}");
            }
            return role;

        }
        public async Task AddRoleAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                throw new ArgumentException("RoleName cannot be empty or whitespace.", nameof(role.RoleName));
            }

            var roleRecord = await _roleRepository.GetAllAsync();
            if (roleRecord.Any(x => x.RoleName.Equals(role.RoleName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"A role with the name '{role.RoleName}' already exists.");
            }

            await _roleRepository.AddAsync(role);
        }
             

      public async Task UpdateRoleAsync(Role role)
        {
            if (role.Id <= 0 || role == null)
            {
                throw new ArgumentException("Role ID must be 0 or Null", nameof(role.Id));
            }
            if (string.IsNullOrWhiteSpace(role.RoleName))
            {
                throw new ArgumentException("RoleName cannot be empty or whitespace.", nameof(role.RoleName));
            }

            var existingRole = await _roleRepository.GetByIdAsync(role.Id);
            if (existingRole == null)
            {
                throw new KeyNotFoundException($"No Role found with ID {role.Id}");
            }

            var roleRecord = await _roleRepository.GetAllAsync();
            if (roleRecord.Any(x => x.RoleName.Equals(role.RoleName, StringComparison.OrdinalIgnoreCase) && x.Id != role.Id))
            {
                throw new ArgumentException($"A role with the name '{role.RoleName}' already exists.");
            }

            await _roleRepository.UpdateAsync(role);
        }

        public async Task DeleteRoleAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Role ID must be greater than 0", nameof(id));
            }

            var existingRole = await _roleRepository.GetByIdAsync(id);
            if (existingRole == null)
            {
                throw new KeyNotFoundException($"No Role found with ID {id}");
            }

            await _roleRepository.DeleteAsync(id);
        }
    }
}
