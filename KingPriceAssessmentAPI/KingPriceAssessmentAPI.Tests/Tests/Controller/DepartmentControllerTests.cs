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
    public class DepartmentControllerTests : IDisposable
    {
        private Mock<IDepartmentService> _departmentServiceMock;
        private DepartmentController _controller;

        [SetUp]
        public void SetUp()
        {
            _departmentServiceMock = new Mock<IDepartmentService>();
            _controller = new DepartmentController(_departmentServiceMock.Object);
        }

     

        [Test]
        public async Task GetAll_ReturnsOkWithDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { Id = 1, DepartmentName = "HR" },
                new Department { Id = 2, DepartmentName = "IT" }
            };
            _departmentServiceMock.Setup(s => s.GetAllDepartmentsAsync()).ReturnsAsync(departments);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(departments));
        }

        [Test]
        public async Task GetById_DepartmentExists_ReturnsOk()
        {
            // Arrange
            var department = new Department { Id = 1, DepartmentName = "HR" };
            _departmentServiceMock.Setup(s => s.GetDepartmentByIdAsync(1)).ReturnsAsync(department);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(department));
        }

        [Test]
        public async Task Add_ValidModel_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var addRequest = new AddDepartmentRequest { DepartmentName = "Finance" };
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Department Added",
            };

            _departmentServiceMock.Setup(s => s.AddDepartmentAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Update_ValidModelAndExists_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var updateRequest = new UpdateDepartmentRequest { Id = 1, DepartmentName = "Updated" };
            var department = new Department { Id = 1, DepartmentName = "HR" };
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Department Added",
            };

            _departmentServiceMock.Setup(s => s.GetDepartmentByIdAsync(1)).ReturnsAsync(department);
            _departmentServiceMock.Setup(s => s.UpdateDepartmentAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Delete_DepartmentExists_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var department = new Department { Id = 1, DepartmentName = "HR" };
            var response = new ResponseMessage
            {
                Success = true,
                Message = "Department Added",
            };

            _departmentServiceMock.Setup(s => s.GetDepartmentByIdAsync(1)).ReturnsAsync(department);
            _departmentServiceMock.Setup(s => s.DeleteDepartmentAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }
        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}