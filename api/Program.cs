using Microsoft.EntityFrameworkCore;
using domain.Context;
using core.Profiles;
using api.Services.Abstractions;
using api.Services;
using api.Helpers;
using System.Text;

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

builder.Services.AddRepositoryGroup();

builder.Services.AddScoped<IJwtTokenService>(x => new JwtTokenService(key));
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
