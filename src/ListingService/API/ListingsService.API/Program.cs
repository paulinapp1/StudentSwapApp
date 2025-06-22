using Microsoft.EntityFrameworkCore;
using ListingsService.Domain.Models;
using ListingsService.API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using UsersService.Domain.Models;
using ListingsService.Domain.Repositories;
using UsersService.Application.Seeders;
using UsersService.Domain.Repositories;
using ListingsService.Application.Seeders;
using ListingsService.Application.Services;
using ListingsService.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Wpisz token w formacie: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<ListingsService.Domain.Models.JwtSettings>(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var rsa = RSA.Create();
    rsa.ImportFromPem(File.ReadAllText("./data/public.key"));
    Console.WriteLine("Public key modulus: " + Convert.ToBase64String(rsa.ExportParameters(false).Modulus));
    var publicKey = new RsaSecurityKey(rsa);

    var jwtConfig = jwtSettings.Get<ListingsService.Domain.Models.JwtSettings>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = publicKey
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Administrator"));
});
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IListingRepository, ListingRepository>();
builder.Services.AddScoped<IAddListingService, AddListingService>();


var app = builder.Build();
using (var migrationScope = app.Services.CreateScope())
{
    try
    {
        var context = migrationScope.ServiceProvider.GetRequiredService<DataContext>();
        context.Database.Migrate(); 
        Console.WriteLine("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        var logger = migrationScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw; 
    }
}
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var categorySeeder = new SeedCategory(
    services.GetRequiredService<IListingRepository>()
  

);
await categorySeeder.SeedCategoryAsync();
var listingSeeder = new SeedListing(
    services.GetRequiredService<IListingRepository>()


);
await listingSeeder.SeedListingAsync();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
