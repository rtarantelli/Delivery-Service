using DeliveryService.Data.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Data
{
    public static class DatabaseInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            DeliveryServiceContext context = serviceProvider.GetRequiredService<DeliveryServiceContext>();
            context.Database.EnsureCreated();

            Point a = new Point { Name = "A" };
            Point b = new Point { Name = "B" };
            Point c = new Point { Name = "C" };
            Point d = new Point { Name = "D" };
            Point e = new Point { Name = "E" };
            Point f = new Point { Name = "F" };
            Point g = new Point { Name = "G" };
            Point h = new Point { Name = "H" };
            Point i = new Point { Name = "I" };

            List<Point> points = new List<Point>() { a, b, c, d, e, f, g, h, i };

            Path AToC = new Path { Origin = a, Destiny = c, Time = 1, Cost = 20 };
            Path AToH = new Path { Origin = a, Destiny = h, Time = 10, Cost = 1 };
            Path AToE = new Path { Origin = a, Destiny = e, Time = 30, Cost = 5 };
            Path EToD = new Path { Origin = e, Destiny = d, Time = 3, Cost = 5 };
            Path DToF = new Path { Origin = d, Destiny = f, Time = 4, Cost = 50 };
            Path FToI = new Path { Origin = f, Destiny = i, Time = 45, Cost = 50 };
            Path FToG = new Path { Origin = f, Destiny = g, Time = 40, Cost = 50 };
            Path GToB = new Path { Origin = g, Destiny = b, Time = 64, Cost = 73 };
            Path IToB = new Path { Origin = i, Destiny = b, Time = 65, Cost = 5 };
            Path CToB = new Path { Origin = c, Destiny = b, Time = 1, Cost = 12 };
            Path HtoE = new Path { Origin = h, Destiny = e, Time = 30, Cost = 1 };

            List<Path> paths = new List<Path>() { AToC, AToH, AToE, EToD, DToF, FToI, FToG, GToB, IToB, IToB, CToB, HtoE };

            Route routeA = new Route { Paths = new List<Path> { AToC, AToE, AToH } };
            Route routeC = new Route { Paths = new List<Path> { CToB } };
            Route routeD = new Route { Paths = new List<Path> { DToF } };
            Route routeE = new Route { Paths = new List<Path> { EToD } };
            Route routeF = new Route { Paths = new List<Path> { FToI, FToG } };
            Route routeG = new Route { Paths = new List<Path> { GToB } };
            Route routeH = new Route { Paths = new List<Path> { HtoE } };
            Route routeI = new Route { Paths = new List<Path> { IToB } };

            List<Route> routes = new List<Route> { routeA, routeC, routeD, routeE, routeF, routeG, routeH, routeI };

            if (!context.Points.Any())
            {
                context.Points.AddRange(points);
                context.SaveChanges();
            }

            if (!context.Paths.Any())
            {
                context.Paths.AddRange(paths);
                context.SaveChanges();
            }

            if (!context.Routes.Any())
            {
                context.Routes.AddRange(routes);
                context.SaveChanges();
            }
        }
    }
}
