using System.Reflection;
using Common.Lib.Extenstion;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Lib.Swagger;

public class SwaggerExcludeFilter : ISchemaFilter
{
    #region ISchemaFilter Members

    public void Apply(OpenApiSchema schema, SchemaFilterContext schemaFilterContext)
    {
        if (schema.Properties == null || schemaFilterContext == null) return;
        var excludedProperties = schemaFilterContext.Type.GetProperties()
            .Where(t => t.GetCustomAttribute(typeof(SwaggerExcludeAttribute), true) != null);
        foreach (var excludedProperty in excludedProperties)
            if (schema.Properties.ContainsKey(excludedProperty.Name.ToCamelCase()))
                schema.Properties.Remove(excludedProperty.Name.ToCamelCase());
    }

    #endregion
}