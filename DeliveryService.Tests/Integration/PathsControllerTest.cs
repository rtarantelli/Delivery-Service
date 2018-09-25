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
    public class PathsControllerTest
    {
        private readonly DeliveryServiceContext _context;
        private readonly IPathRepository _pathRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IRouteRepository _routeRepository;

        public PathsControllerTest()
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

        [Theory]
        [InlineData(200)]
        public void Paths_Get_All_StatusCode_200(int status)
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            IActionResult result = controller.GetPaths();

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(400)]
        public void Paths_Get_All_StatusCode_400(int status)
        {
            IPathRepository pathRepository = null;
            IPointRepository pointRepository = null;
            PathsController controller = new PathsController(pathRepository, pointRepository);

            IActionResult result = controller.GetPaths();

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(200)]
        public void Paths_Get_Id_StatusCode_200(int status)
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            IActionResult result = controller.GetPath(1);

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(400)]
        public void Paths_Get_Id_StatusCode_400(int status)
        {
            IPathRepository pathRepository = null;
            IPointRepository pointRepository = null;
            PathsController controller = new PathsController(pathRepository, pointRepository);

            IActionResult result = controller.GetPath(1);

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(404)]
        public void Paths_Get_Id_StatusCode_404(int status)
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            IActionResult result = controller.GetPath(100);

            int? statusCode = ((NotFoundResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(201)]
        public void Paths_Add_StatusCode_201(int status)
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
        [InlineData(400)]
        public void Paths_Add_StatusCode_400(int status)
        {
            IPathRepository pathRepository = null;
            IPointRepository pointRepository = null;

            PathsController controller = new PathsController(pathRepository, pointRepository);

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
        [InlineData(202)]
        public void Paths_Update_StatusCode_202(int status)
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
        [InlineData(400)]
        public void Paths_Update_StatusCode_400(int status)
        {
            IPathRepository pathRepository = null;
            IPointRepository pointRepository = null;

            PathsController controller = new PathsController(pathRepository, pointRepository);

            Path path = new Path()
            {
                Destiny = new Point() { Name = "A" },
                Origin = new Point() { Name = "B" }
            };

            IActionResult actionResult = controller.PutPath(path.PathId, path);

            int? statusCode = ((BadRequestObjectResult)(actionResult.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(202)]
        public void Paths_Delete_StatusCode_202(int status)
        {
            PathsController controller = new PathsController(_pathRepository, _pointRepository);

            IActionResult actionResult = controller.DeletePath(1);

            int? statusCode = ((ObjectResult)actionResult.Should().Subject).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(400)]
        public void Paths_Delete_StatusCode_400(int status)
        {
            IPathRepository pathRepository = null;
            IPointRepository pointRepository = null;
            PathsController controller = new PathsController(pathRepository, pointRepository);

            IActionResult actionResult = controller.DeletePath(1);

            int? statusCode = ((ObjectResult)actionResult.Should().Subject).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

    }
}
