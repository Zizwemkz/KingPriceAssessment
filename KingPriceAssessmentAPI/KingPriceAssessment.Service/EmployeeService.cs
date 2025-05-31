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

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Employee ID must be greater than 0", nameof(id));
            }

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"No Employee found with ID {id}");
            }
            return employee;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {

            var allEmployees = await _employeeRepository.GetAllAsync();
            if (allEmployees.Any(e => e.EmployeeNumber.Equals(employee.EmployeeNumber, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"An employee with the number '{employee.EmployeeNumber}' already exists.");
            }

            await _employeeRepository.AddAsync(employee);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            if (employee.Id <= 0)
            {
                throw new ArgumentException("Employee ID must be greater than 0", nameof(employee.Id));
            }

            var existingEmployee = await _employeeRepository.GetByIdAsync(employee.Id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"No Employee found with ID {employee.Id}");
            }

            var allEmployees = await _employeeRepository.GetAllAsync();
            if (allEmployees.Any(e => e.EmployeeNumber.Equals(employee.EmployeeNumber, StringComparison.OrdinalIgnoreCase) && e.Id != employee.Id))
            {
                throw new ArgumentException($"An employee with the number '{employee.EmployeeNumber}' already exists.");
            }

            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Employee ID must be greater than 0", nameof(id));
            }

            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"No Employee found with ID {id}");
            }

            await _employeeRepository.DeleteAsync(id);
        }
    }
}
