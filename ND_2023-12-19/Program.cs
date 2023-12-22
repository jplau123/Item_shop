using Microsoft.EntityFrameworkCore;
using ND_2023_12_19.Contexts;
using ND_2023_12_19.Interfaces;
using ND_2023_12_19.Repositories;
using ND_2023_12_19.Services;
using project_backend.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string dbConnectonString = builder.Configuration.GetConnectionString("item_db")
    ?? throw new ArgumentException("Connection string cannot be found.");

// Dapper connection
//builder.Services.AddScoped<IDbConnection>((serviceProvider) => new NpgsqlConnection(dbConnectonString));

// Ef Core connection
builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(dbConnectonString));

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IItemRepository, ItemRepositoryEfCore>();

builder.Services.AddTransient<ErrorMiddleware>();

builder.Services.AddAutoMapper(typeof(Program));

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
