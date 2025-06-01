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
    public class EmployeeAllocationControllerTests : IDisposable
    {
        private Mock<IEmployeeAllocationService> _employeeAllocationServiceMock;
        private EmployeeAllocationController _controller;

        [SetUp]
        public void SetUp()
        {
            _employeeAllocationServiceMock = new Mock<IEmployeeAllocationService>();
            _controller = new EmployeeAllocationController(_employeeAllocationServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkWithAllocations()
        {
            // Arrange
            var allocations = new List<EmployeeAllocation>
            {
                new EmployeeAllocation { Id = 1, EmployeeId = 2, DepartmentId = 3, RoleId = 4 },
                new EmployeeAllocation { Id = 2, EmployeeId = 3, DepartmentId = 4, RoleId = 5 }
            };
            _employeeAllocationServiceMock.Setup(s => s.GetAllAllocationsAsync()).ReturnsAsync(allocations);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(allocations));
        }

        [Test]
        public async Task GetById_AllocationExists_ReturnsOk()
        {
            // Arrange
            var allocation = new EmployeeAllocation { Id = 1, EmployeeId = 2, DepartmentId = 3, RoleId = 4 };
            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync(allocation);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(allocation));
        }

        [Test]
        public async Task GetById_AllocationNotFound_ReturnsNotFound()
        {
            // Arrange
            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync((EmployeeAllocation)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Add_ValidModel_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var addRequest = new AddEmployeeAllocationRequest { EmployeeId = 2, DepartmentId = 3, RoleId = 4 };
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Allocation Added"
            };

            _employeeAllocationServiceMock.Setup(s => s.AddAllocationAsync(addRequest)).ReturnsAsync(response);

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
            _controller.ModelState.AddModelError("EmployeeId", "Required");

            // Act
            var result = await _controller.Add(new AddEmployeeAllocationRequest());

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Add_ServiceReturnsFailure_ReturnsBadRequest()
        {
            // Arrange
            var addRequest = new AddEmployeeAllocationRequest { EmployeeId = 2, DepartmentId = 3, RoleId = 4 };
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeAllocationServiceMock.Setup(s => s.AddAllocationAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_ValidModelAndExists_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var updateRequest = new UpdateEmployeeAllocationRequest { Id = 1, EmployeeId = 2, DepartmentId = 3, RoleId = 4 };
            var allocation = new EmployeeAllocation { Id = 1, EmployeeId = 2, DepartmentId = 3, RoleId = 4 };
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Allocation Updated"
            };

            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync(allocation);
            _employeeAllocationServiceMock.Setup(s => s.UpdateAllocationAsync(updateRequest)).ReturnsAsync(response);

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
            var updateRequest = new UpdateEmployeeAllocationRequest { Id = 2, EmployeeId = 2, DepartmentId = 3, RoleId = 4 };

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_AllocationNotFound_ReturnsNotFound()
        {
            // Arrange
            var updateRequest = new UpdateEmployeeAllocationRequest { Id = 1, EmployeeId = 2, DepartmentId = 3, RoleId = 4 };
            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync((EmployeeAllocation)null);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Update_ServiceReturnsFailure_ReturnsBadRequest()
        {
            // Arrange
            var updateRequest = new UpdateEmployeeAllocationRequest { Id = 1, EmployeeId = 2, DepartmentId = 3, RoleId = 4 };
            var allocation = new EmployeeAllocation { Id = 1, EmployeeId = 2, DepartmentId = 3, RoleId = 4 };
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync(allocation);
            _employeeAllocationServiceMock.Setup(s => s.UpdateAllocationAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Delete_AllocationExists_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var allocation = new EmployeeAllocation { Id = 1, EmployeeId = 2, DepartmentId = 3, RoleId = 4 };
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Allocation Deleted"
            };

            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync(allocation);
            _employeeAllocationServiceMock.Setup(s => s.DeleteAllocationAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Delete_AllocationNotFound_ReturnsNotFound()
        {
            // Arrange
            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync((EmployeeAllocation)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_ServiceReturnsFailure_ReturnsBadRequest()
        {
            // Arrange
            var allocation = new EmployeeAllocation { Id = 1, EmployeeId = 2, DepartmentId = 3, RoleId = 4 };
            var response = new ResponseMessage
            {
                Success = false,
                Message = "Failed"
            };

            _employeeAllocationServiceMock.Setup(s => s.GetAllocationByIdAsync(1)).ReturnsAsync(allocation);
            _employeeAllocationServiceMock.Setup(s => s.DeleteAllocationAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task GetEmployeesByDepartment_ValidDepartment_ReturnsOk()
        {
            // Arrange
            string departmentName = "HR";
            var employees = new List<EmployeeDetailsDto>
            {
                new EmployeeDetailsDto { EmployeeNumber = "1", Name = "John", Lastname = "Doe", RoleName = "Manager",DepartmentName = "HR" },
                new EmployeeDetailsDto { EmployeeNumber = "2", Name = "Jane", Lastname = "Smith", RoleName = "Consultant", DepartmentName = "IT" }
            };

            _employeeAllocationServiceMock.Setup(s => s.GetEmployeesByDepartmentNameAsync(departmentName)).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetEmployeesByDepartment(departmentName) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(employees));
        }

        [Test]
        public async Task GetEmployeesByDepartment_DepartmentNameMissing_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetEmployeesByDepartment(null);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}