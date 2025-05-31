using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Common.Repository
{
    public interface IRoleRepository
    {
        public Task<IEnumerable<Role>> GetAllAsync();
        public Task<Role> GetByIdAsync(int id);
        public Task AddAsync(Role employee);
        public Task UpdateAsync(Role employee);
        public Task DeleteAsync(int id);
    }
}
