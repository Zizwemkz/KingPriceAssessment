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
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
            => _employeeRepository.GetAllAsync();

        public Task<Employee> GetEmployeeByIdAsync(int id)
            => _employeeRepository.GetByIdAsync(id);

        public Task AddEmployeeAsync(Employee employee)
            => _employeeRepository.AddAsync(employee);

        public Task UpdateEmployeeAsync(Employee employee)
            => _employeeRepository.UpdateAsync(employee);

        public Task DeleteEmployeeAsync(int id)
            => _employeeRepository.DeleteAsync(id);
    }
}
