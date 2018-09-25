using DeliveryService.Api.Controllers;
using DeliveryService.Data;
using DeliveryService.Data.Interface;
using DeliveryService.Data.Model;
using DeliveryService.Data.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace DeliveryService.Tests
{
    public class PointsControllerTests
    {
        private readonly DeliveryServiceContext _context;
        private readonly IPointRepository _pointRepository;

        public PointsControllerTests()
        {
            ServiceProvider serviceProvider = CreateServiceProvider();

            IServiceProvider service = serviceProvider.GetService<IServiceProvider>();
            _context = service.GetRequiredService<DeliveryServiceContext>();
            _pointRepository = new PointRepository(_context) as IPointRepository;

            DatabaseInitializer.Seed(service);
        }

        private static ServiceProvider CreateServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddTransient<IPointRepository, PointRepository>();
            services.AddDbContext<DeliveryServiceContext>(opttions => opttions.UseInMemoryDatabase("DeliveryServiceContext"));

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        [Theory]
        [InlineData(200)]
        public void Points_Get_All_StatusCode_200(int status)
        {
            PointsController controller = new PointsController(_pointRepository);

            IActionResult result = controller.GetPoints();

            int? actionResult = ((ObjectResult)result.Should().Subject).StatusCode;

            actionResult.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(400)]
        public void Points_Get_All_StatusCode_400(int status)
        {
            IPointRepository repository = null;
            PointsController controller = new PointsController(repository);

            IActionResult result = controller.GetPoints();

            int? actionResult = ((ObjectResult)result.Should().Subject).StatusCode;

            actionResult.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(200)]
        public void Points_Get_Id_StatusCode_200(int status)
        {
            PointsController controller = new PointsController(_pointRepository);

            IActionResult result = controller.GetPoint(1);

            int? statusCode = ((OkObjectResult)result.Should().Subject).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(400)]
        public void Points_Get_Id_StatusCode_400(int status)
        {
            IPointRepository pointRepository = null;
            PointsController controller = new PointsController(pointRepository);

            IActionResult result = controller.GetPoint(1);

            int? statusCode = ((BadRequestObjectResult)result.Should().Subject).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(404)]
        public void Points_Get_Id_StatusCode_404(int status)
        {
            PointsController controller = new PointsController(_pointRepository);

            IActionResult result = controller.GetPoint(100);

            int statusCode = ((NotFoundResult)result.Should().Subject).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(201)]
        public void Points_Add_StatusCode_201(int status)
        {
            PointsController controller = new PointsController(_pointRepository);

            Point newPoint = new Point() { Name = "K" };

            IActionResult result = controller.PostPoint(newPoint).Should().BeOfType<CreatedAtActionResult>().Subject;

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(400)]
        public void Points_Add_StatusCode_400(int status)
        {
            IPointRepository pointRepository = null;
            PointsController controller = new PointsController(pointRepository);

            Point newPoint = new Point() { Name = "K" };

            IActionResult result = controller.PostPoint(newPoint);

            BadRequestObjectResult actionResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;

            actionResult.StatusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(400)]
        public void Points_Update_StatusCode_400(int status)
        {
            PointsController controller = new PointsController(_pointRepository);

            Point point = new Point() { Name = "K", PointId = 10 };

            IActionResult result = controller.PutPoint(point.PointId, point);

            BadRequestObjectResult actionResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;

            actionResult.StatusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(202)]
        public void Points_Delete_StatusCode_202(int status)
        {
            PointsController controller = new PointsController(_pointRepository);

            Point point = new Point() { Name = "K", PointId = 10 };

            IActionResult result = controller.DeletePoint(point.PointId);

            AcceptedResult actionResult = result.Should().BeOfType<AcceptedResult>().Subject;

            actionResult.StatusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(400)]
        public void Points_Delete_StatusCode_400(int status)
        {
            IPointRepository pointRepository = null;
            PointsController controller = new PointsController(pointRepository);

            Point point = new Point() { Name = "K", PointId = 10 };

            IActionResult result = controller.DeletePoint(point.PointId);

            BadRequestObjectResult actionResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;

            actionResult.StatusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(404)]
        public void Points_Delete_StatusCode_404(int status)
        {
            PointsController controller = new PointsController(_pointRepository);

            Point point = new Point() { Name = "K", PointId = 100 };

            IActionResult result = controller.DeletePoint(point.PointId);

            NotFoundResult actionResult = result.Should().BeOfType<NotFoundResult>().Subject;

            actionResult.StatusCode.Should().Be(status, $"StatusCode should be {status}");
        }
    }
}
