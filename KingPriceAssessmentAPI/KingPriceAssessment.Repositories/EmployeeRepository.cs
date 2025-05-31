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
        private readonly EmployeeDbContext EmployeeDb;

        public EmployeeRepository(EmployeeDbContext context)
        {
            EmployeeDb = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                return await EmployeeDb.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Employee ID must be greater than 0", nameof(id));

            try
            {
                var employee = await EmployeeDb.Employees.FindAsync(id);
                if (employee == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

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
                if (await EmployeeDb.Employees.AnyAsync(e => e.EmployeeNumber == employee.EmployeeNumber))
                    throw new ArgumentException("An employee with the same EmployeeNumber already exists.");

                await EmployeeDb.Employees.AddAsync(employee);
                await EmployeeDb.SaveChangesAsync();
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
                var existingEmployee = await EmployeeDb.Employees.FindAsync(employee.Id);
                if (existingEmployee == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);


                EmployeeDb.Employees.Update(employee);
                await EmployeeDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the Employee.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Employee ID must be greater than 0", nameof(id));
            }

            try
            {
                var emp = await EmployeeDb.Employees.FindAsync(id);
                if (emp != null)
                {
                    EmployeeDb.Employees.Remove(emp);
                    await EmployeeDb.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the Employee.", ex);
            }
        }
    }
}
