using Microsoft.EntityFrameworkCore;
using core.Context;
using core.Profiles;
using core.Repositories;
using core.Repositories.Abstractions;
using api.Services.Abstractions;
using api.Services;
using api.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.UTF8.GetBytes(appSettings.Secret);

builder.Services.AddJwtAuthentication(key);

builder.Services.AddAutoMapper(typeof(UserProfile), typeof(OrderProfile));

builder.Services.AddDbContext<CargoDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CargoDBContext")));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<ICompaniesRepository, CompaniesRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICargoTypesRepository, CargoTypesRepository>();
builder.Services.AddScoped<IPaymentMethodsRepository, PaymentMethodsRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
