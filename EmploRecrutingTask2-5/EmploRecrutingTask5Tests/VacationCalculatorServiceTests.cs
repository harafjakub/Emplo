using EmploRecrutingTask2_5.Models;
using EmploRecrutingTask2_5.Repositories;
using EmploRecrutingTask2_5.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmploRecrutingTask5Tests
{
    public class VacationCalculatorServiceTests
    {
        private readonly Mock<ILogger<VacationCalculatorService>> _mockLogger;

        public VacationCalculatorServiceTests()
        {
            _mockLogger = new Mock<ILogger<VacationCalculatorService>>();
        }

        private VacationCalculatorService CreateService(Mock<IEmployeeRepository> mockRepository)
        {
            return new VacationCalculatorService(mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task employee_can_request_vacation()
        {
            // Arrange
            var currentYear = DateTime.Now.Year;

            var vacationPackage = new VacationPackage
            {
                Id = 1,
                Name = "Standard Package",
                GrantedDays = 20,
                Year = currentYear
            };

            var employee = new Employee
            {
                Id = 1,
                Name = "Test Employee",
                TeamId = 1,
                PositionId = 1,
                VacationPackageId = 1,
                VacationPackage = vacationPackage
            };

            var vacations = new List<Vacation>
            {
                new()
                {
                    Id = 1,
                    DateSince = new DateTime(currentYear, 1, 1),
                    DateUntil = new DateTime(currentYear, 1, 5),
                    EmployeeId = 1,
                    Employee = employee
                }
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetEmployeeWithVacationPackageAsync(1))
                .ReturnsAsync(employee);
            mockRepository.Setup(r => r.GetEmployeeVacationsAsync(1))
                .ReturnsAsync(vacations);

            var service = CreateService(mockRepository);

            // Act
            var result = await service.IfEmployeeCanRequestVacationAsync(1);

            // Assert
            Assert.True(result);
            mockRepository.Verify(r => r.GetEmployeeWithVacationPackageAsync(1), Times.Once);
            mockRepository.Verify(r => r.GetEmployeeVacationsAsync(1), Times.Once);
        }

        [Fact]
        public async Task employee_cant_request_vacation()
        {
            // Arrange
            var currentYear = DateTime.Now.Year;

            var vacationPackage = new VacationPackage
            {
                Id = 2,
                Name = "Standard Package",
                GrantedDays = 10,
                Year = currentYear
            };

            var employee = new Employee
            {
                Id = 2,
                Name = "Test Employee 2",
                TeamId = 1,
                PositionId = 1,
                VacationPackageId = 2,
                VacationPackage = vacationPackage
            };

            var vacations = new List<Vacation>
            {
                new()
                {
                    Id = 2,
                    DateSince = new DateTime(currentYear, 1, 1),
                    DateUntil = new DateTime(currentYear, 1, 5),
                    EmployeeId = 2,
                    Employee = employee
                },
                new()
                {
                    Id = 3,
                    DateSince = new DateTime(currentYear, 2, 1),
                    DateUntil = new DateTime(currentYear, 2, 5),
                    EmployeeId = 2,
                    Employee = employee
                }
            };

            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetEmployeeWithVacationPackageAsync(2))
                .ReturnsAsync(employee);
            mockRepository.Setup(r => r.GetEmployeeVacationsAsync(2))
                .ReturnsAsync(vacations);

            var service = CreateService(mockRepository);

            // Act
            var result = await service.IfEmployeeCanRequestVacationAsync(2);

            // Assert
            Assert.False(result);
            mockRepository.Verify(r => r.GetEmployeeWithVacationPackageAsync(2), Times.Once);
            mockRepository.Verify(r => r.GetEmployeeVacationsAsync(2), Times.Once);
        }
    }
}