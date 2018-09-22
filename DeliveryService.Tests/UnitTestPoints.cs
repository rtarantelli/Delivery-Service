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
    public class UnitTestPoints
    {
        private readonly DeliveryServiceContext _context;
        private readonly IPointRepository _pointRepository;

        public UnitTestPoints()
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
        public void Points_Get_All_StatusCode(int status)
        {
            PointsController controller = new PointsController(_pointRepository);

            IActionResult result = controller.GetPoints();

            int? actionResult = ((ObjectResult)result.Should().Subject).StatusCode;

            actionResult.Should().Be(status, $"StatusCode should be {status}");
        }

        [Fact]
        public void Points_Get_Specific_StatusCode()
        {
            PointsController controller = new PointsController(_pointRepository);

            IActionResult result = controller.GetPoint(1);

            int? actionResult = ((ObjectResult)result.Should().Subject).StatusCode;

            actionResult.Should().Be(200, $"StatusCode should be 200");
        }

        [Theory]
        [InlineData(1)]
        public void Points_Get_Specific(int id)
        {
            PointsController controller = new PointsController(_pointRepository);

            IActionResult result = controller.GetPoint(id);

            OkObjectResult actionResult = result.Should().BeOfType<OkObjectResult>().Subject;

            Point point = actionResult.Value.Should().BeAssignableTo<Point>().Subject;

            point.PointId.Should().Be(id, $"Point.PointId should be {id}");
        }

        [Theory]
        [InlineData(201)]
        public void Points_Add_StatusCode(int status)
        {
            PointsController controller = new PointsController(_pointRepository);

            Point newPoint = new Point() { Name = "K" };

            IActionResult result = controller.PostPoint(newPoint).Should().BeOfType<CreatedAtActionResult>().Subject;

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(11)]
        public void Points_Add(int id)
        {
            PointsController controller = new PointsController(_pointRepository);

            Point newPoint = new Point() { Name = "K" };

            IActionResult result = controller.PostPoint(newPoint);

            CreatedAtActionResult actionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;

            Point point = actionResult.Value.Should().BeAssignableTo<Point>().Subject;

            point.PointId.Should().Be(id, $"Point.PointId should be {id}");
        }

        [Theory]
        [InlineData(400)]
        public void Points_Update_StatusCode(int status)
        {
            PointsController controller = new PointsController(_pointRepository);

            Point point = new Point() { Name = "K", PointId = 10 };

            IActionResult result = controller.PutPoint(point.PointId, point);

            BadRequestObjectResult actionResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;

            actionResult.StatusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Fact]
        public void Points_Delete()
        {
            PointsController controller = new PointsController(_pointRepository);

            Point point = new Point() { Name = "K", PointId = 10 };

            IActionResult result = controller.DeletePoint(point.PointId);

            AcceptedResult actionResult = result.Should().BeOfType<AcceptedResult>().Subject;

            actionResult.StatusCode.Should().Be(202, $"Point cannot be deleted");
        }
    }
}
