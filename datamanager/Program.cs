using application.Interfaces;
using application.Services;
using domain.Entities;
using domain.Interfaces;
using infrastructure.Factory;
using infrastructure.Repository;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository<Template>, RequirementRepository>();
builder.Services.AddScoped<IConnectioDataBase, SqlServerDataBase>();
builder.Services.AddScoped<IConnectioDataBase, CosmosDataBase>();
builder.Services.AddScoped<IConnectionFactory, ConnectionFactory>();
builder.Services.AddScoped<IRequirement, RequirementService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
