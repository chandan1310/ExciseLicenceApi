using Microsoft.EntityFrameworkCore;
using ExciseLicenceApi.Data;
using Scalar.AspNetCore;

// Add this line to resolve the AddSwaggerGen and UseSwagger errors
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen(); // <-- Should resolve now

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    // Enable classic Swagger UI middleware
    app.UseSwagger();   // <-- Should resolve now
    app.UseSwaggerUI();  // <-- Should resolve now
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();