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

            builder.Services.AddControllers(configure =>
            {
                configure.ReturnHttpNotAcceptable = true;
            }).AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            //CORS Policy configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.WithOrigins("*") // Add here all the origins that you want to allow.
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials(); // This allows cookies to be sent with the CORS requests.
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


            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseResponseCaching();

            // app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();


            return app;
        }
    }
}
