using Microsoft.OpenApi.Models;

namespace DrinkVendingMachine.Api.Options;

public record SwaggerDocOptions(string Name, string Version, string Title, string Description, OpenApiServer[] Servers);