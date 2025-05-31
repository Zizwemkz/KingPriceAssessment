using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using KingPriceAssessment.Common.Repository;
using KingPriceAssessment.Data;
using KingPriceAssessment.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace KingPriceAssessment.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public EmployeeRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                return await _employeeDbContext.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            try
            {
                var employee = await _employeeDbContext.Employees.FindAsync(id);
                return employee;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public async Task AddAsync(Employee employee)
        {
            if (employee == null) 
            {
                throw new ArgumentNullException(nameof(employee));
            }
 
            try
            {
                await _employeeDbContext.Employees.AddAsync(employee);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
        public async Task UpdateAsync(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            try
            {
                _employeeDbContext.Employees.Update(employee);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the Employee.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var emp = await _employeeDbContext.Employees.FindAsync(id);
                _employeeDbContext.Employees.Remove(emp);
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the Employee.", ex);
            }
        }
    }
}
