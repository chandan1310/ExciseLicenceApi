using Microsoft.EntityFrameworkCore;
using ExciseLicenceApi.Data;
using Scalar.AspNetCore;

// Add this line to resolve the AddSwaggerGen and UseSwagger errors
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// 1. ADD CORS POLICY (Must be declared before building the app)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularAndSwagger", policy =>
    {
        policy.AllowAnyOrigin()   // Allows Angular (localhost:4200) or Swagger/Scalar interfaces
              .AllowAnyMethod()   // Allows POST, GET, PUT, DELETE, etc.
              .AllowAnyHeader();  // Allows content-type, authorization headers
    });
});

// Force Kestrel to use fresh, unblocked ports
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8081); // HTTP
    options.ListenAnyIP(8082, listenOptions => listenOptions.UseHttps());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlServrConnStrng")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register BOTH OpenAPI and Swagger generation support
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. ENABLE CORS MIDDLEWARE (Must be placed before MapControllers)
app.UseCors("AllowAngularAndSwagger");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    // Enable classic Swagger UI middleware
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Commented out temporarily if testing purely on HTTP port 8081
app.MapControllers();
app.Run();