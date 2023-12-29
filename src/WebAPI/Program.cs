
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Clients;
using Infrastructure.Repositories;
using Npgsql;
using System.Data;
using WebAPI.Extensions;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string dbConnectonString = builder.Configuration.GetConnectionString("item_db")
    ?? throw new ArgumentException("Connection string cannot be found.");

// Dapper connection
builder.Services.AddScoped<IDbConnection>((serviceProvider) => new NpgsqlConnection(dbConnectonString));

// Ef Core connection
//builder.Services.AddDbContext<DataContext>(opt => opt.UseNpgsql(dbConnectonString));

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<IJsonPlaceholderClient, JsonPlaceholderClient>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserCacheRepository, UserCacheRepository>();
builder.Services.AddScoped<IUserCacheService, UserCacheService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddTransient<ErrorMiddleware>();

builder.Services.AddAutoMapper(typeof(Application.Profiles.AutoMapper));
builder.Services.AddHttpClient();

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

app.UseErrorMiddleware();

app.Run();
