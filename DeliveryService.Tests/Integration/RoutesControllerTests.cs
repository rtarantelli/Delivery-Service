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
using System.Collections.Generic;
using Xunit;

namespace DeliveryService.Tests
{
    public class RoutesControllerTests
    {
        private readonly DeliveryServiceContext _context;
        private readonly IPathRepository _pathRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IRouteRepository _routeRepository;

        public RoutesControllerTests()
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
            services.AddTransient<IRouteRepository, RouteRepository>();
            services.AddDbContext<DeliveryServiceContext>(opttions => opttions.UseInMemoryDatabase("DeliveryServiceContext"));

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        [Theory]
        [InlineData(200)]
        public void Routes_Get_All_StatusCode_200(int status)
        {
            RoutesController controller = new RoutesController(_pathRepository, _pointRepository, _routeRepository);

            IActionResult result = controller.GetRoutes();

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(404)]
        public void Routes_Get_All_StatusCode_404(int status)
        {
            IRouteRepository routeRepository = null;
            RoutesController controller = new RoutesController(_pathRepository, _pointRepository, routeRepository);

            IActionResult result = controller.GetRoutes();

            int? statusCode = ((NotFoundResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(200)]
        public void Routes_Get_Id_StatusCode_200(int status)
        {
            RoutesController controller = new RoutesController(_pathRepository, _pointRepository, _routeRepository);

            IActionResult result = controller.GetRoutes("A", "A", 'A');

            int? statusCode = ((ObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData(400)]
        public void Routes_Get_Id_StatusCode_400(int status)
        {
            IRouteRepository routeRepository = null;
            RoutesController controller = new RoutesController(_pathRepository, _pointRepository, routeRepository);

            IActionResult result = controller.GetRoutes("A", "A", 'A');

            int? statusCode = ((BadRequestObjectResult)(result.Should().Subject)).StatusCode;

            statusCode.Should().Be(status, $"StatusCode should be {status}");
        }

        [Theory]
        [InlineData("A", "B", "C")]
        public void Paths_Get_Specific(params string[] values)
        {
            RoutesController controller = new RoutesController(_pathRepository, _pointRepository, _routeRepository);

            IActionResult result = controller.GetRoutes(values[0], values[1], char.Parse(values[2]));

            List<Route> route = ((ObjectResult)(result.Should().Subject)).Value.As<List<Route>>();

            route.Should().HaveCountGreaterThan(0, $"Routes total should be greater than 0");
        }
    }
}
