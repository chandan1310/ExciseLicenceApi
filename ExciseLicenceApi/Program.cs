using Microsoft.EntityFrameworkCore;
using ExciseLicenceApi.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register your Database Context using the connection string from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlServrConnStrng")));

// 2. Add Controller support so your LicenceApplicationsController is recognized
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 3. Add native OpenAPI support
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Generates the raw openapi/v1.json file
    app.MapOpenApi();

    // Turns on the visual UI dashboard at /scalar/v1
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// 4. Map your attribute-routed controllers (like api/LicenceApplications)
app.MapControllers();

app.Run();