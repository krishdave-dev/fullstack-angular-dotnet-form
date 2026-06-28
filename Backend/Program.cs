using Backend.Data;
using Backend.Interfaces;
using Backend.Middlewares;
using Backend.Repositories;
using Backend.Repositories.ADO;
using Backend.Repositories.EFCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseSqlServer(
            builder.Configuration
            .GetConnectionString(
                "DefaultConnection"));
    });
    builder.Services.AddScoped<DbConnectionFactory>();
builder.Services.AddScoped<CustomerAdoRepository>();
builder.Services.AddScoped<CustomerEfRepository>();
builder.Services.AddScoped<IRepositoryResolver, RepositoryResolver>();
// Add services to the container
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    // Configured with options arrow function =>
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Serves Swagger at the root URL
    });
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers(); 

app.Run();
