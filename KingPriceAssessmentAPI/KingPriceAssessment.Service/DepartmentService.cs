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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public Task<IEnumerable<Department>> GetAllDepartmentsAsync()
            => _departmentRepository.GetAllAsync();

        public Task<Department> GetDepartmentByIdAsync(int id)
            => _departmentRepository.GetByIdAsync(id);

        public Task AddDepartmentAsync(Department department)
            => _departmentRepository.AddAsync(department);

        public Task UpdateDepartmentAsync(Department department)
            => _departmentRepository.UpdateAsync(department);

        public Task DeleteDepartmentAsync(int id)
            => _departmentRepository.DeleteAsync(id);
    }
}
