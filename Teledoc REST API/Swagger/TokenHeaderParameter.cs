using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection.Metadata;

namespace Teledoc_REST_API.Swagger
{
    public class TokenHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            // Заголовок для Токена
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Token",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }
    }
}
