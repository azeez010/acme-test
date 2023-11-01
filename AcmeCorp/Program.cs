using AcmeCorp.Core.Service;
using AcmeCorp.Domain;
using AcmeCorp.Domain.Interface.Repository;
using AcmeCorp.Domain.Interface.Service;
using AcmeCorp.Infrastructure.DataContext;
using AcmeCorp.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresConnection")
    , b => b.MigrationsAssembly("AcmeCorp.API")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<IContactInfoRepository, ContactInfoRepository>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();



builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IContactInfoService, ContactInfoService>();
builder.Services.AddTransient<IOrderService, OrderService>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ACMECORP API", Description = "Acme Corp System API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer Scheme.
                        Enter 'Bearer'[space] and then your token in the text input below.
                        Example: Bearer 12345abcdef ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"},
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                },
                new List<string>()
                }
            });
});

builder.Services.Configure<JwtTokenConfig>(
    builder.Configuration.GetSection("JwtTokenConfig"));

string jwtSecret = builder.Configuration.GetSection("JwtTokenConfig:Secret").Value;
string jwtIssuer = builder.Configuration.GetSection("JwtTokenConfig:Issuer").Value;
string jwtAudience = builder.Configuration.GetSection("JwtTokenConfig:Audience").Value;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience
            };
        }
);


builder.Services.AddAuthentication();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ALLOW",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyMethod()
            .AllowAnyHeader();
        });
});

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
