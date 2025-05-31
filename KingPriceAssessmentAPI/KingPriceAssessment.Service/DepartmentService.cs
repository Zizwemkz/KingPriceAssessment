using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Department ID must be greater than 0", nameof(id));
            }

            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                throw new KeyNotFoundException($"No Department found with ID {id}");
            }
            return department;
        }

        public async Task AddDepartmentAsync(Department department)
        {
            if (department == null)
            {
                throw new ArgumentNullException(nameof(department));
            }

            if (string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                throw new ArgumentException("DepartmentName cannot be empty or whitespace.", nameof(department.DepartmentName));
            }

            var allDepartments = await _departmentRepository.GetAllAsync();
            if (allDepartments.Any(d => d.DepartmentName.Equals(department.DepartmentName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"A department with the name '{department.DepartmentName}' already exists.");
            }

            await _departmentRepository.AddAsync(department);
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            if (department.Id <= 0 || department ==  null)
            {
                throw new ArgumentException("Department ID must be greater than 0", nameof(department.Id));
            }
            if (string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                throw new ArgumentException("DepartmentName cannot be empty or whitespace.", nameof(department.DepartmentName));
            }

            var existingDepartment = await _departmentRepository.GetByIdAsync(department.Id);
            if (existingDepartment == null)
            {
                throw new KeyNotFoundException($"No Department found with ID {department.Id}");
            }

            var allDepartments = await _departmentRepository.GetAllAsync();
            if (allDepartments.Any(d => d.DepartmentName.Equals(department.DepartmentName, StringComparison.OrdinalIgnoreCase) && d.Id != department.Id))
            {
                throw new ArgumentException($"A department with the name '{department.DepartmentName}' already exists.");
            }

            await _departmentRepository.UpdateAsync(department);
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Department ID must be greater than 0", nameof(id));
            }

            var existingDepartment = await _departmentRepository.GetByIdAsync(id);
            if (existingDepartment == null)
            {
                throw new KeyNotFoundException($"No Department found with ID {id}");
            }

            await _departmentRepository.DeleteAsync(id);
        }
    }
}