using System.Security.Cryptography;
using System.Text.Json.Serialization;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserCrud.API.Handlers;
using UserCrud.Application;
using UserCrud.Application.Interfaces;
using UserCrud.Infrastructure;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddUseCases();
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

var publicKey = Environment.GetEnvironmentVariable("JWT_PUBLIC_KEY");
var keyBytes = Convert.FromBase64String(publicKey);

var rsa = RSA.Create();
rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);
var rsaKey = new RsaSecurityKey(rsa);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            IssuerSigningKey = rsaKey,
            ClockSkew = TimeSpan.Zero
        };
        
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["autoria_token"];
                
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                
                return Task.CompletedTask;
            }
        };
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options));

builder.Services.AddOpenApi();
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
            .Select(s => s.Split('?')[0].Split('{')[0].Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();
        
        if (segments.Length <= 1 || !segments[0].Equals("api", StringComparison.OrdinalIgnoreCase))
        {
            return [segments.FirstOrDefault() ?? "Default"];
        }
        
        var groupName = segments[1];
        
        return [char.ToUpper(groupName[0]) + groupName[1..]];

    });
    
    options.OrderActionsBy(api => api.RelativePath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.GetRequiredService<IEnvironmentVariablesService>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();
app.Run();
