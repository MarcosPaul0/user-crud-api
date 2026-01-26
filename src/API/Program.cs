using System.Text.Json.Serialization;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UserCrud.API.Middlewares;
using UserCrud.Application.UseCases;
using UserCrud.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddUseCases();
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddOpenApi(options =>
{
    // Specify the OpenAPI version to use
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.TagActionsBy(api =>
    {
        var routeTemplate = api.RelativePath;
        
        if (string.IsNullOrEmpty(routeTemplate))
            return ["Default"];

        var segments = routeTemplate
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Split('?')[0].Split('{')[0].Trim()) // Remove query strings e parâmetros de rota
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();
        
        // Verifica se começa com 'api' e tem mais segmentos
        if (segments.Length <= 1 || !segments[0].Equals("api", StringComparison.OrdinalIgnoreCase))
        {
            return [segments.FirstOrDefault() ?? "Default"];
        }
        
        var groupName = segments[1];
            
        // Capitaliza a primeira letra
        return [char.ToUpper(groupName[0]) + groupName[1..]];

    });
    
    options.OrderActionsBy(api => api.RelativePath);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("JwtSettings", options))
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();
