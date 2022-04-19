using Microsoft.EntityFrameworkCore;
using core.Context;
using core.Profiles;
using core.Repositories;
using core.Repositories.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Dto mapping
builder.Services.AddAutoMapper(typeof(UserProfile), typeof(OrderProfile));
// Database context and repositories DI
builder.Services.AddDbContext<CargoDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CargoDBContext")));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<ICompaniesRepository, CompaniesRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICargoTypesRepository, CargoTypesRepository>();
builder.Services.AddScoped<IPaymentMethodsRepository, PaymentMethodsRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();

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
