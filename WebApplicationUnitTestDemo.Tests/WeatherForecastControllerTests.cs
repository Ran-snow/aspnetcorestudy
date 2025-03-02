using Moq;

using WebApplicationUnitTestDemo.Controllers;
using WebApplicationUnitTestDemo.Repositories;

using Xunit.Abstractions;

namespace WebApplicationUnitTestDemo.Tests
{
    public class WeatherForecastControllerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WeatherForecastControllerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GetForecast_ReturnsOkResultWithForecast()
        {
            // Arrange
            var mockService = new Mock<IReservedRepository>();
            mockService.Setup(s => s.GetSummary("123"))
                       .Returns("ssss");

            var controller = new WeatherForecastController(mockService.Object);

            // Act
            var result = controller.Get();

            // Assert
            var forecasts = Assert.IsAssignableFrom<IEnumerable<WeatherForecast>>(result);
            Assert.True(forecasts.Count() == 5);

            _testOutputHelper.WriteLine("OK");
        }
    }
}