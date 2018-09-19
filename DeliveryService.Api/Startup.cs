using DeliveryService.Data;
using DeliveryService.Data.Interface;
using DeliveryService.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;

namespace DeliveryService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ConfigureServiceContext(services);

            ConfigureRepositoriesDependencyInjection(services);

            ConfigureSwaggerGen(services);

            ConfigureAuthProvider(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();

            ConfigureSwaggerUI(app);
        }

        private void ConfigureAuthProvider(IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
        }

        private static void ConfigureServiceContext(IServiceCollection services)
        {
            services.AddDbContext<DeliveryServiceContext>(opttions => opttions.UseInMemoryDatabase("DeliveryServiceContext"));
        }

        private static void ConfigureRepositoriesDependencyInjection(IServiceCollection services)
        {
            services.AddTransient<IPointRepository, PointRepository>();
            services.AddTransient<IPathRepository, PathRepository>();
            services.AddTransient<IRouteRepository, RouteRepository>();
        }

        private static void ConfigureSwaggerGen(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Contact = new Contact()
                    {
                        Email = "renato.tarantelli@live.com",
                        Name = "Renato Tarantelli",
                        Url = "https://www.linkedin.com/in/renatotarantelli/"
                    },
                    Description = "Management of best time or cost to delivery",
                    License = new License()
                    {
                        Name = "Apache License",
                        Url = "http://www.apache.org/licenses/"
                    },
                    Title = "Delivery Service API",
                    TermsOfService = "None",
                    Version = "v1"
                });
            });
        }

        private static void ConfigureSwaggerUI(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery Services - v1");
            });

            try
            {
                using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    ServiceContext context = serviceScope.ServiceProvider.GetRequiredService<ServiceContext>();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }
    }
}
