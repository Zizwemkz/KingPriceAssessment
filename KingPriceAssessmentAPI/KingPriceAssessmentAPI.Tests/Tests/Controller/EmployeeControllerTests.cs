using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KingPriceAssessment.Common.Interfaces.Service;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Models.Response;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessmentAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace KingPriceAssessmentAPI.Tests.Controllers
{
    [TestFixture]
    public class EmployeeControllerTests : IDisposable
    {
        private Mock<IEmployeeService> _employeeServiceMock;
        private EmployeeController _controller;

        [SetUp]
        public void SetUp()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_employeeServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkWithEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John", Lastname = "Doe" },
                new Employee { Id = 2, Name = "Jane", Lastname = "Smith" }
            };
            _employeeServiceMock.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(employees));
        }

        [Test]
        public async Task GetById_EmployeeExists_ReturnsOk()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John", Lastname = "Doe" };
            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(employee));
        }

        [Test]
        public async Task GetById_EmployeeNotFound_ReturnsNotFound()
        {
            // Arrange
            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Add_ValidModel_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var addRequest = new AddEmployeeRequest { Name = "John", Lastname = "Doe" };
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Employee Added"
            };

            _employeeServiceMock.Setup(s => s.AddEmployeeAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Add_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Add(new AddEmployeeRequest());

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Add_ServiceReturnsFailure_ReturnsBadRequest()
        {
            // Arrange
            var addRequest = new AddEmployeeRequest { Name = "John", Lastname = "Doe" };
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeServiceMock.Setup(s => s.AddEmployeeAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_ValidModelAndExists_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var updateRequest = new UpdateEmployeeRequest { Id = 1, Name = "Updated", Lastname = "User" };
            var employee = new Employee { Id = 1, Name = "John", Lastname = "Doe" };
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Employee Updated"
            };

            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(s => s.UpdateEmployeeAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var updateRequest = new UpdateEmployeeRequest { Id = 2, Name = "Updated", Lastname = "User" };

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_EmployeeNotFound_ReturnsNotFound()
        {
            // Arrange
            var updateRequest = new UpdateEmployeeRequest { Id = 1, Name = "Updated", Lastname = "User" };
            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Update_ServiceReturnsFailure_ReturnsBadRequest()
        {
            // Arrange
            var updateRequest = new UpdateEmployeeRequest { Id = 1, Name = "Updated", Lastname = "User" };
            var employee = new Employee { Id = 1, Name = "John", Lastname = "Doe" };
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(s => s.UpdateEmployeeAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Delete_EmployeeExists_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John", Lastname = "Doe" };
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Employee Deleted"
            };

            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Delete_EmployeeNotFound_ReturnsNotFound()
        {
            // Arrange
            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_ServiceReturnsFailure_ReturnsBadRequest()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John", Lastname = "Doe" };
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeServiceMock.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(employee);
            _employeeServiceMock.Setup(s => s.DeleteEmployeeAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}