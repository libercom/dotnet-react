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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.AddAutoMapper(typeof(UserProfile), typeof(OrderProfile), typeof(CargoTypeProfile),
    typeof(CountryProfile), typeof(CompanyProfile), typeof(PaymentMethodProfile), typeof(RoleProfile));

builder.Services.AddDbContext<CargoDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CargoDBContext")));

builder.Services.AddLogging();

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
app.UseRouting();
app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseFileServer();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute("Spa", "{*url}", defaults: new { controller = "Home", action = "Spa" });
});

/*app.MapControllers();*/

app.Run();
