using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pos.Application;
using Pos.Application.Configuration;
using Pos.Api.Infrastructure;
using Pos.Domain.Interfaces.Repositories;
using Pos.Domain.Security;
using Pos.Infrastructure;
using Pos.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationDependency();
builder.Services.AddControllers();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSecret = jwtSection["Secret"];
if (string.IsNullOrWhiteSpace(jwtSecret))
{
    throw new InvalidOperationException("Configura una clave JWT en 'Jwt:Secret'.");
}

if (!builder.Environment.IsDevelopment() &&
    jwtSecret.StartsWith("CHANGE_ME_", StringComparison.Ordinal))
{
    throw new InvalidOperationException("Configura una clave JWT segura para producción.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization(options =>
{
    static bool HasOwnerOrPermission(AuthorizationHandlerContext context, string permissionCode)
    {
        if (context.User.HasClaim("isOwner", "true"))
            return true;

        return context.User.Claims.Any(c =>
            string.Equals(c.Type, "perm", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(c.Value, permissionCode, StringComparison.OrdinalIgnoreCase));
    }

    static void AddPermissionPolicy(AuthorizationOptions options, string policyName, string permissionCode)
    {
        options.AddPolicy(policyName, policy =>
            policy.RequireAuthenticatedUser()
                .RequireAssertion(ctx => HasOwnerOrPermission(ctx, permissionCode)));
    }

    AddPermissionPolicy(options, PermissionCodes.ProductsRead, PermissionCodes.ProductsRead);
    AddPermissionPolicy(options, PermissionCodes.ProductsCreate, PermissionCodes.ProductsCreate);
    AddPermissionPolicy(options, PermissionCodes.ProductsUpdate, PermissionCodes.ProductsUpdate);
    AddPermissionPolicy(options, PermissionCodes.ProductsDelete, PermissionCodes.ProductsDelete);
    AddPermissionPolicy(options, PermissionCodes.CashBoxRead, PermissionCodes.CashBoxRead);
    AddPermissionPolicy(options, PermissionCodes.CashBoxOpen, PermissionCodes.CashBoxOpen);
    AddPermissionPolicy(options, PermissionCodes.CashBoxClose, PermissionCodes.CashBoxClose);
    AddPermissionPolicy(options, PermissionCodes.CashFlowsRead, PermissionCodes.CashFlowsRead);
    AddPermissionPolicy(options, PermissionCodes.CashFlowsCreate, PermissionCodes.CashFlowsCreate);
    AddPermissionPolicy(options, PermissionCodes.UsersRead, PermissionCodes.UsersRead);
    AddPermissionPolicy(options, PermissionCodes.UsersCreate, PermissionCodes.UsersCreate);
    AddPermissionPolicy(options, PermissionCodes.UsersUpdate, PermissionCodes.UsersUpdate);
    AddPermissionPolicy(options, PermissionCodes.UsersDelete, PermissionCodes.UsersDelete);
    AddPermissionPolicy(options, PermissionCodes.PermissionsRead, PermissionCodes.PermissionsRead);
    AddPermissionPolicy(options, PermissionCodes.PermissionsManage, PermissionCodes.PermissionsManage);
    AddPermissionPolicy(options, PermissionCodes.UserActivityRead, PermissionCodes.UserActivityRead);
    AddPermissionPolicy(options, PermissionCodes.ReportsProfitsRead, PermissionCodes.ReportsProfitsRead);
    AddPermissionPolicy(options, PermissionCodes.ReportsCashCutsRead, PermissionCodes.ReportsCashCutsRead);

    options.AddPolicy("CanManagePermissions", policy =>
        policy.RequireAuthenticatedUser()
            .RequireAssertion(ctx =>
                HasOwnerOrPermission(ctx, PermissionCodes.PermissionsManage)));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://localhost:1420",
                "https://tauri.localhost",
                "tauri://localhost")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "POS API",
        Version = "v1",
        Description = "API para el sistema POS",
        Contact = new OpenApiContact { Name = "POS Team", Email = "support@pos.local" }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT en header. Ej: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

var isEfDesignTime = AppDomain.CurrentDomain.GetAssemblies()
    .Any(a => string.Equals(a.GetName().Name, "Microsoft.EntityFrameworkCore.Design", StringComparison.Ordinal));

if (!isEfDesignTime)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PosDbContext>();
    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var logger = scope.ServiceProvider
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("DatabaseMigration");

    var autoMigrateEnabled = app.Configuration.GetValue("Database:AutoMigrate", true);
    try
    {
        if (autoMigrateEnabled)
        {
            var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Count > 0)
            {
                logger.LogInformation("Se detectaron {Count} migraciones pendientes. Aplicando...", pendingMigrations.Count);
                dbContext.Database.Migrate();
                logger.LogInformation("Migraciones pendientes aplicadas correctamente.");
            }
            else
            {
                logger.LogInformation("No hay migraciones pendientes.");
            }
        }

        await userRepository.EnsurePermissionCatalogAsync();
        logger.LogInformation("Catálogo de permisos verificado/sincronizado.");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Error durante inicialización de base de datos/permisos al iniciar.");
        throw;
    }
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Backen Api");

app.Run();
