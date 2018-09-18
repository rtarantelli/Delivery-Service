using DeliveryService.Data.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
            context.Points.AddRange(points);

            Path AToC = new Path { Origin = a, Destiny = c };
            Path AToE = new Path { Origin = a, Destiny = e };
            Path AToH = new Path { Origin = a, Destiny = h };
            Path CToB = new Path { Origin = c, Destiny = b };
            Path DToF = new Path { Origin = d, Destiny = f };
            Path EToD = new Path { Origin = e, Destiny = d };
            Path FToG = new Path { Origin = f, Destiny = g };
            Path FToI = new Path { Origin = f, Destiny = i };
            Path GToB = new Path { Origin = g, Destiny = b };
            Path HtoE = new Path { Origin = h, Destiny = e };
            Path IToB = new Path { Origin = i, Destiny = b };

            List<Path> paths = new List<Path>() { AToC, AToH, AToE, CToB, DToF, EToD, FToG, FToI, GToB, HtoE, IToB };
            context.Paths.AddRange(paths);

            Route routeAToC = new Route { Cost = 01, Time = 20, PathId = 1 };
            Route routeAToE = new Route { Cost = 30, Time = 05, PathId = 2 };
            Route routeAToH = new Route { Cost = 10, Time = 01, PathId = 3 };
            Route routeCToB = new Route { Cost = 01, Time = 12, PathId = 4 };
            Route routeDToF = new Route { Cost = 04, Time = 50, PathId = 5 };
            Route routeEToD = new Route { Cost = 03, Time = 05, PathId = 6 };
            Route routeFToG = new Route { Cost = 40, Time = 50, PathId = 7 };
            Route routeFToI = new Route { Cost = 45, Time = 50, PathId = 8 };
            Route routeGToB = new Route { Cost = 64, Time = 73, PathId = 9 };
            Route routeHtoE = new Route { Cost = 30, Time = 01, PathId = 10 };
            Route routeIToB = new Route { Cost = 65, Time = 05, PathId = 11 };

            List<Route> routes = new List<Route> { routeAToC, routeAToE, routeAToH, routeCToB, routeDToF, routeEToD, routeFToG, routeFToI, routeGToB, routeHtoE, routeIToB };
            context.Routes.AddRange(routes);

            context.SaveChanges();
        }
    }
}
