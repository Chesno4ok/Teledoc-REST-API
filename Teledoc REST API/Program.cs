
using Microsoft.OpenApi.Models;
using System.Reflection;
using Teledoc_REST_API.Middlewares;
using Teledoc_REST_API.Services;
using Teledoc_REST_API.Swagger;

namespace Teledoc_REST_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddAutoMapper(typeof(AppMappingProfile));

            

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                // Добавление в Swagger заголовка
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTestService", Version = "v1", });
                c.OperationFilter<TokenHeaderParameter>();

            });



            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<AuthorizationMiddleware>();

            app.UseMiddleware<ParamCheckMiddleware>();
            
            app.MapControllers();

            app.Run();
        }
    }
}
