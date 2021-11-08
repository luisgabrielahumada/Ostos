
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Filter
{
    public class SwaggerExcludeSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            foreach (var item in context.SchemaRepository.Schemas.Keys.Where(r =>  r.Contains("Extensions") || r.Contains("Filter") || r.Contains("Activator") || r.Contains("Attribute") || r.Contains("Http") || r.Contains("State")))
                context.SchemaRepository.Schemas.Remove(item);

            //context.SchemaRepository.Schemas.Remove("JToken");
            //context.SchemaRepository.Schemas.Remove("WeatherForecast2DtoListHttpMessage");
        }
    }
}
