using LoanAgent.Api.Constants;
using LoanAgent.Api.FilterAttributes;
using LoanAgent.Api.JsonConverters;
using LoanAgent.Api.Swagger;
using LoanAgent.Application;
using LoanAgent.Infrastructure;
using LoanAgent.Infrastructure.SignalRHubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Serilog;

using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<ApiExceptionFilterAttribute>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    });

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHealthChecks();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressInferBindingSourcesForParameters = true;
});

builder.Services.AddCors(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.AddAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddAuthorizationWithPolicy();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

try
{
    Log.Information("Starting up the application...");

    app.UseCors();

    app.UseRouting();

    app.UseSwagger(app.Environment);

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapHub<LoanHub>("/loanHub");

    app.MapControllers();

    app.MapHealthChecks("/healthz");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

internal static class ServiceCollectionExtensions
{
    public static void AddAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
    {
        _ = services
            .AddAuthentication()
            .AddJwtBearer(AuthenticationSchemas.LOAN_AGENT, options =>
            {
                var issuer = configuration["JwtToken:Issuer"];
                var audience = configuration["JwtToken:Audience"];

                options.RequireHttpsMetadata = !environment.IsDevelopment();

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtToken:SecretKey"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = ClaimTypes.Role
                };
            });
    }

    public static void AddSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            options.DocumentFilter<SwaggerDocumentFilter>();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "LoanAgent API"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a JWT token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");

            options.EnableAnnotations();

            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly)
                    .Where(f => f.Contains("LoanAgent", StringComparison.OrdinalIgnoreCase));

            foreach (var xmlFile in xmlFiles)
                options.IncludeXmlComments(xmlFile);
        });

    public static void AddCors(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

    public static void AddAuthorizationWithPolicy(
        this IServiceCollection services) =>
        services
        .AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(AuthenticationSchemas.LOAN_AGENT)
                .Build();
        });
}

internal static class WebAppExtensions
{
    public static void UseSwagger(
        this WebApplication app,
        IWebHostEnvironment environment)
    {
        app.UseSwagger(options =>
        {
            options.RouteTemplate = environment.IsProduction()
                ? "/loanagent-swagger-prod/{documentname}/swagger.json"
                : "/loanagent-swagger/{documentname}/swagger.json";

            if (!environment.IsDevelopment())
                options.PreSerializeFilters.Add((swaggerDoc, _) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new()
                        {
                            Url = $"{app.Configuration.GetValue<string>("ApiGatewayUrl")}"
                        }
                    };
                });
        });

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("v1/swagger.json", "LoanAgent API");

            options.RoutePrefix = environment.IsProduction()
                ? "loanagent-swagger-prod"
                : "loanagent-swagger";
        });
    }
}