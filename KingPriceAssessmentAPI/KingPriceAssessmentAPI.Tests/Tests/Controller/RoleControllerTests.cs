using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using System.Web.Http.Results;
using KingPriceAssessment.Common.Interfaces.Service;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Tables;
using KingPriceAssessmentAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using KingPriceAssessment.Data.Models.Response;

namespace KingPriceAssessmentAPI.Tests.Controllers
{
    [TestFixture]
    public class RoleControllerTests : IDisposable
    {
        private Mock<IRoleService> _roleServiceMock;
        private RoleController _controller;

        [SetUp]
        public void SetUp()
        {
            _roleServiceMock = new Mock<IRoleService>();
            _controller = new RoleController(_roleServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkWithRoles()
        {
            // Arrange
            var response = new List<Role>
            {
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "User" }
            };
            _roleServiceMock.Setup(s => s.GetAllRolesAsync()).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task GetById_RoleExists_ReturnsOk()
        {
            // Arrange
            var role = new Role { Id = 1, RoleName = "Admin" };
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(role));
        }

        [Test]
        public async Task GetById_RoleNotFound_ReturnsNotFound()
        {
            // Arrange
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync((Role)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.NotFoundResult>());
        }

        [Test]
        public async Task Add_ValidModel_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var addRequest = new AddRoleRequest { RoleName = "Finance" };
            var response = new ResponseMessage { Success = true, Message = "Added" };

            _roleServiceMock.Setup(s => s.AddRoleAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Add_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("RoleName", "Required");

            // Act
            var result = await _controller.Add(new AddRoleRequest());

            // Assert
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>());
        }

        [Test]
        public async Task Add_ServiceReturnsFailure_ReturnsBadRequest()
        {
            // Arrange
            var addRequest = new AddRoleRequest { RoleName = "Finance" };
            var response = new ResponseMessage { Success = false, Message = "Failed" };

            _roleServiceMock.Setup(s => s.AddRoleAsync(addRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Add(addRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_ValidModelAndExists_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var updateRequest = new UpdateRoleRequest { Id = 1, RoleName = "Updated" };
            var role = new Role { Id = 1, RoleName = "Admin" };
            var response = new ResponseMessage { Success = true, Message = "Updated" };

            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);
            _roleServiceMock.Setup(s => s.UpdateRoleAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var updateRequest = new UpdateRoleRequest { Id = 2, RoleName = "Updated" };

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_RoleNotFound_ReturnsNotFound()
        {
            // Arrange
            var updateRequest = new UpdateRoleRequest { Id = 1, RoleName = "Updated" };
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync((Role)null);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.NotFoundResult>());
        }

        [Test]
        public async Task Update_ServiceReturnsFailure_ReturnsBadRequest()
        {
            // Arrange
            var updateRequest = new UpdateRoleRequest { Id = 1, RoleName = "Updated" };
            var role = new Role { Id = 1, RoleName = "Admin" };
            var response = new ResponseMessage { Success = false, Message = "Failed" };

            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);
            _roleServiceMock.Setup(s => s.UpdateRoleAsync(updateRequest)).ReturnsAsync(response);

            // Act
            var result = await _controller.Update(1, updateRequest);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>());
        }

        [Test]
        public async Task Delete_RoleExists_ReturnsOkWithSuccessResponse()
        {
            // Arrange
            var role = new Role { Id = 1, RoleName = "Admin" };
            var response = new ResponseMessage { Success = true, Message = "Deleted" };

            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);
            _roleServiceMock.Setup(s => s.DeleteRoleAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Delete_RoleNotFound_ReturnsNotFound()
        {
            // Arrange
            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync((Role)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.NotFoundResult>());
        }

        [Test]
        public async Task Delete_ServiceReturnsFailure_ReturnsBadRequest()
        {
            // Arrange
            var role = new Role { Id = 1, RoleName = "Admin" };
            var response = new ResponseMessage { Success = false, Message = "Failed" };

            _roleServiceMock.Setup(s => s.GetRoleByIdAsync(1)).ReturnsAsync(role);
            _roleServiceMock.Setup(s => s.DeleteRoleAsync(1)).ReturnsAsync(response);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>());
        }

        [Test]
        public async Task GetRolesByDepartment_ValidDepartment_ReturnsOk()
        {
            // Arrange
            var departmentName = "Finance";
            var response = new List<DepartmentRolesDto>
            {
                new DepartmentRolesDto
                {
                    DepartmentName = departmentName,
                    Roles = new List<string> { "Analyst", "Manager" }
                }
            };
            _roleServiceMock.Setup(s => s.GetRolesForDepartmentAsync(departmentName)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetRolesByDepartment(departmentName) as OkObjectResult;

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task GetRolesByDepartment_DepartmentNameMissing_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetRolesByDepartment(null);

            // Assert
            Assert.That(result, Is.InstanceOf<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>());
        }
        public void Dispose()
        {
            _controller?.Dispose();
        }
    }
}