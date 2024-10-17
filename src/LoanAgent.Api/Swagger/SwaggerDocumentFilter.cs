using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace LoanAgent.Api.Swagger;

public class SwaggerDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        ArgumentNullException.ThrowIfNull(swaggerDoc, nameof(swaggerDoc));

        // No need to show routes containing "qa"
        foreach (var item in swaggerDoc.Paths.Where(p => p.Key.StartsWith("/qa/")))
            swaggerDoc.Paths.Remove(item.Key);
    }
}
