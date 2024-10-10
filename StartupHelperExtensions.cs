using AppBuilderDataAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace AppBuilderDataAPI
{
    public static class StartupHelperExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            //Swagger API documentation service
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            builder.Services.AddControllers();
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    name:"AllowAll",
                    policy =>
                    {
                       policy.AllowAnyOrigin() 
                             .AllowAnyHeader()
                             .AllowAnyMethod();
                          
                    });
                });

            // Database connection configuration
            builder.Services.AddDbContext<AppBuilderDataDbContext>(options =>
            {
                    options.UseInMemoryDatabase("AppBuilderDataDb");
            });

            //AutoMapper Dependency Injection
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Adding services

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // Build the app
            var app = builder.Build();

            // Seed the database
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppBuilderDataDbContext>();
                dbContext.Seed();  // Call the Seed method
            }

            return app;
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //app.UseResponseCaching();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();


            return app;
        }
    }
}
