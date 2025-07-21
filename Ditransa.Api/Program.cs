using AutoMapper;
using Ditransa.Api.Common;
using Ditransa.Api.Middlewares;
using Ditransa.Application.Common.Mappings;
using Ditransa.Application.Extensions;
using Ditransa.Application.Interfaces.Repositories;
using Ditransa.Infrastructure.Extensions;
using Ditransa.Persistence.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddHttpClient();

builder.Services.AddControllers(options =>
{
    // Registrar el filtro global
    options.Filters.Add<ApiResponseWrapperFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Add Authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JwtBearer").GetSection("Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JwtBearer").GetSection("Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtBearer").GetSection("SecretKey").Value!)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    options.OnRejected = (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.Headers["Retry-After"] = "180";

        var responseObject = new
        {
            message = "Demasiadas solicitudes. Intenta nuevamente en unos minutos."
        };

        string json = JsonSerializer.Serialize(responseObject);

        return new ValueTask(context.HttpContext.Response.WriteAsync(json, cancellationToken));
    };

    options.AddPolicy("LoginPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anon",
            factory: key => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(3),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
       builder => builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetPreflightMaxAge(TimeSpan.FromDays(5)));
});

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
    mc.AddProfile(new Ditransa.Infrastructure.Mapper.MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddAuthorization(options =>
{
    //var optionRepository = builder.Services.BuildServiceProvider().GetService<IOptionRepository>();
    //var optionsList = optionRepository.GetAllAsync().GetAwaiter().GetResult().Where(c => !string.IsNullOrEmpty(c.Code)).ToList();

    //foreach (var item in optionsList)
    //{
    //    options.AddPolicy(item.Code, policy => policy.RequireClaim(item.Code));
    //}
});
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()

    .WriteTo.Console()

    // Archivo SOLO para lógica de negocio (Information & Warning)
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(le =>
            le.Level == Serilog.Events.LogEventLevel.Information ||
            le.Level == Serilog.Events.LogEventLevel.Warning
        )
        .WriteTo.File(
            "Logs/business-log.txt",
            rollingInterval: RollingInterval.Infinite
        )
    )

    // Archivo SOLO para errores
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(le =>
            le.Level == Serilog.Events.LogEventLevel.Error ||
            le.Level == Serilog.Events.LogEventLevel.Fatal
        )
        .WriteTo.File(
            "Logs/error-log.txt",
            rollingInterval: RollingInterval.Infinite
        )
    )

    .CreateLogger();


builder.Host.UseSerilog();
var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseRateLimiter();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();