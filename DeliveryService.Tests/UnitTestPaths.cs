using DeliveryService.Data;
using DeliveryService.Data.Interface;
using DeliveryService.Data.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace DeliveryService.Tests
{
    public class UnitTestPaths
    {
        private readonly DeliveryServiceContext _context;

        public UnitTestPaths()
        {
            ServiceProvider serviceProvider = CreateServiceProvider();

            IServiceProvider service = serviceProvider.GetService<IServiceProvider>();
            _context = service.GetRequiredService<DeliveryServiceContext>();

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
        [InlineData(9)]
        public void ShouldBeCountAsTotal(int total)
        {
            var items = _context.Paths.Count();

            items.Should().Be(total, $"Total should be {total}");
        }

        [Fact]
        public void ShouldGetAllPoints()
        {
        }
    }
}
