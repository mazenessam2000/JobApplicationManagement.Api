using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using JobApplicationManagement.Api.Authentication;
using JobApplicationManagement.Application.Behaviors;
using JobApplicationManagement.Application.Exceptions;
using JobApplicationManagement.Application.Persistence;
using JobApplicationManagement.Application.Queries;
using JobApplicationManagement.Application.Services;
using JobApplicationManagement.Infrastructure.Identity;
using JobApplicationManagement.Infrastructure.Persistence;
using JobApplicationManagement.Infrastructure.Persistence.Queries;
using JobApplicationManagement.Infrastructure.Persistence.Repositories;
using JobApplicationManagement.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

builder.Services.AddOptions<JwtOptions>()
    .BindConfiguration(JwtOptions.SectionName)
    .ValidateDataAnnotations()
    .Validate(options => Encoding.UTF8.GetByteCount(options.Key) >= 32, "JWT signing key must be at least 32 bytes.")
    .ValidateOnStart();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobQueries, JobQueries>();
builder.Services.AddScoped<IJobApplicationQueries, JobApplicationQueries>();
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 12;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
});
builder.Services.AddSingleton<JwtTokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<IOptions<JwtOptions>>((options, jwtOptions) =>
{
    var jwt = jwtOptions.Value;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, ValidIssuer = jwt.Issuer,
        ValidateAudience = true, ValidAudience = jwt.Audience,
        ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
        ValidateLifetime = true, ClockSkew = TimeSpan.FromMinutes(1)
    };
});
builder.Services.AddAuthorization();
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(IJobRepository).Assembly);
    options.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(IJobRepository).Assembly);

var app = builder.Build();
app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    if (exception is not null)
        context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("GlobalExceptionHandler")
            .LogError(exception, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);
    var (status, title, errors) = exception switch
    {
        ValidationException validation => (StatusCodes.Status400BadRequest, "Validation failed", validation.Errors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage))),
        KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found", null),
        ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden", null),
        UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized", null),
        BusinessRuleException => (StatusCodes.Status409Conflict, "Business rule violation", null),
        _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", null)
    };
    context.Response.StatusCode = status;
    await context.Response.WriteAsJsonAsync(new { title, detail = status == 500 ? null : exception?.Message, errors }, context.RequestAborted);
}));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
