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
    public class UnitTestPaths
    {
        private readonly DeliveryServiceContext _context;
        private readonly IPathRepository _pathRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IRouteRepository _routeRepository;

        public UnitTestPaths()
        {
            ServiceProvider serviceProvider = CreateServiceProvider();

            IServiceProvider service = serviceProvider.GetService<IServiceProvider>();
            _context = service.GetRequiredService<DeliveryServiceContext>();
            _pathRepository = new PathRepository(_context) as IPathRepository;
            _pointRepository = new PointRepository(_context) as IPointRepository;
            _routeRepository = new RouteRepository(_context) as IRouteRepository;

            DatabaseInitializer.Seed(service);
        }

        private static ServiceProvider CreateServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddTransient<IPointRepository, PointRepository>();
            services.AddTransient<IPathRepository, PathRepository>();
            services.AddDbContext<DeliveryServiceContext>(opttions => opttions.UseInMemoryDatabase("DeliveryServiceContext"));

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        [Fact]
        public void Paths_Get_All()
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            IActionResult result = controller.GetPaths();

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(200, "StatusCode should be 200");
        }

        [Theory]
        [InlineData(1)]
        public void Paths_Get_Specific(int id)
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            IActionResult result = controller.GetPath(id);

            Path path = ((ObjectResult)(result.Should().Subject)).Value.As<Path>();

            path.PathId.Should().Be(id, $"Path.PathId should be {id}");
        }

        [Fact]
        public void Paths_Get_Specific_StatusCode()
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            IActionResult result = controller.GetPath(1);

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(200, "StatusCode should be 200");
        }

        [Theory]
        [InlineData(201)]
        public void Paths_Add_StatusCode(int status)
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            Path path = new Path()
            {
                DestinyId = 1,
                OriginId = 2
            };

            IActionResult result = controller.PostPath(path);

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(12)]
        public void Paths_Add_Result(int id)
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            Path path = new Path()
            {
                DestinyId = 1,
                OriginId = 2
            };

            IActionResult result = controller.PostPath(path);

            path = ((ObjectResult)(result.Should().Subject)).Value.As<Path>();

            path.PathId.Should().Be(id, $"Path.PathId should be {id}");
        }

        [Theory]
        [InlineData(202)]
        public void Paths_Update_StatusCode(int status)
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            IActionResult result = controller.GetPath(1);

            Path path = ((ObjectResult)controller.GetPath(1)).Value.As<Path>();

            Point point = new Point() { Name = "K", PointId = 11 };
            path.DestinyId = point.PointId;
            path.Destiny = point;

            IActionResult actionResult = controller.PutPath(path.PathId, path);

            int? statusCode = ((ObjectResult)(actionResult.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(202)]
        public void Paths_Delete(int status)
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            Path path = ((ObjectResult)controller.GetPath(1)).Value.As<Path>();

            IActionResult actionResult = controller.DeletePath(path.PathId);

            int? statusCode = ((ObjectResult)actionResult.Should().Subject).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }
    }
}
