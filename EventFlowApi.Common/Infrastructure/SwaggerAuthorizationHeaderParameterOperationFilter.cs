using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EventFlowApi.Common.Infrastructure
{
    public class SwaggerAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Parameters?.Add(new NonBodyParameter
            {
                Name = "Authorization",
                In = "header",
                Description = "Access Token",
                Required = false,
                Type = "string"
            });
        }
    }
}
